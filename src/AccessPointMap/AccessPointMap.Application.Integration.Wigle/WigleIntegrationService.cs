using AccessPointMap.Application.Integration.Core;
using AccessPointMap.Application.Integration.Core.Exceptions;
using AccessPointMap.Application.Integration.Wigle.Models;
using AccessPointMap.Application.Oui.Core;
using AccessPointMap.Application.Pcap.Core;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Infrastructure.Core.Abstraction;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Integration.Wigle
{
    public class WigleIntegrationService : AccessPointIntegrationBase<WigleIntegrationService>, IWigleIntegrationService
    {
        private readonly string _allowedType = "WIFI";

        private readonly string _adnnotationName = "WiGLE integration provided data";

        private const string _integrationName = "WiGLE";
        private const string _integrationDescription = "Integration for the bigest wardriving platform and their scanning application";
        private const string _integrationVersion = "1.1.0";

        private const string _csvPreheader = $"WigleWifi-1.4,appRelease=1.0.0,model=AccessPointMap,release=AccessPointMap,device=AccessPointMap,display=AccessPointMap,board=AccessPointMap,brand=AccessPointMap";

        private const double _defaultFrequencyValue = default;

        protected override string IntegrationName => _integrationName;
        protected override string IntegrationDescription => _integrationDescription;
        protected override string IntegrationVersion => _integrationVersion;

        public WigleIntegrationService(
            IUnitOfWork unitOfWork,
            IDataAccess dataAccess,
            IScopeWrapperService scopeWrapperService,
            IPcapParsingService pcapParsingService,
            IOuiLookupService ouiLookupService) : base(unitOfWork, dataAccess, scopeWrapperService, pcapParsingService, ouiLookupService) { }

        public async Task Handle(IIntegrationCommand command)
        {
            switch (command)
            {
                case Commands.CreateAccessPointsFromCsvFile cmd: await HandleCommand(cmd); break;
                case Commands.CreateAccessPointsFromCsvGzFile cmd: await HandleCommand(cmd); break;

                default: throw new IntegrationException($"This command is not supported by the {IntegrationName} integration.");
            }
        }

        public async Task<object> Query(IIntegrationQuery query)
        {
            switch (query)
            {
                case Queries.ExportAccessPointsToCsv q: return await HandleQuery(q);

                default: throw new IntegrationException($"This query is not supported by the {IntegrationName} integration.");
            }
        }

        private async Task HandleCommand(Commands.CreateAccessPointsFromCsvFile cmd)
        {
            if (cmd.ScanCsvFile is null)
                throw new ArgumentNullException(nameof(cmd));

            if (Path.GetExtension(cmd.ScanCsvFile.FileName).ToLower() != ".csv")
                throw new ArgumentNullException(nameof(cmd));

            using var sr = new StreamReader(cmd.ScanCsvFile.OpenReadStream());

            // This line will shift the stream by one line to avoid the header
            _ = sr.ReadLine();

            using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<AccessPointRecord>()
                .GroupBy(r => r.Mac, (k, v) => CombineRecords(v))
                .ToList();

            await HandleAccessPointRecords(records);
        }

        private async Task HandleCommand(Commands.CreateAccessPointsFromCsvGzFile cmd)
        {
            if (cmd.ScanCsvGzFile is null)
                throw new ArgumentNullException(nameof(cmd));

            if (!cmd.ScanCsvGzFile.FileName.ToLower().EndsWith(".csv.gz"))
                throw new ArgumentNullException(nameof(cmd));

            using var compressionStream = new GZipStream(cmd.ScanCsvGzFile.OpenReadStream(), CompressionMode.Decompress);
            using var sr = new StreamReader(compressionStream);

            // This line will shift the stream by one line to avoid the header
            _ = sr.ReadLine();

            using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<AccessPointRecord>()
                .GroupBy(r => r.Mac, (k, v) => CombineRecords(v))
                .ToList();

            await HandleAccessPointRecords(records);
        }

        private async Task HandleAccessPointRecords(IEnumerable<AccessPointRecord> accessPointRecords)
        {
            foreach (var record in accessPointRecords)
            {
                if (!record.Type.ToUpper().Contains(_allowedType)) continue;

                if (await UnitOfWork.AccessPointRepository.Exists(record.Mac))
                {
                    await CreateAccessPointStamp(record);
                    continue;
                }

                await CreateAccessPoint(record);
            }

            await UnitOfWork.Commit();
        }

        private async Task<object> HandleQuery(Queries.ExportAccessPointsToCsv q)
        {
            var accessPoints = DataAccess.AccessPoints
                .Where(a => !a.DeletedAt.HasValue)
                .Where(a => a.DisplayStatus.Value)
                .ToList();

            var records = accessPoints
                .Select(a => AccessPointToRecord(a));

            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream, Encoding.UTF8);

            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            foreach (var field in _csvPreheader.Split(',')) csv.WriteField(field);
            await csv.NextRecordAsync();

            await csv.WriteRecordsAsync(records);

            return stream.ToArray();
        }

        private async Task CreateAccessPoint(AccessPointRecord record)
        {
            var accessPoint = AccessPoint.Factory.Create(new Events.V1.AccessPointCreated
            {
                Bssid = record.Mac,
                Ssid = record.Ssid,
                Frequency = _defaultFrequencyValue,
                LowSignalLevel = record.Rssi,
                LowSignalLatitude = record.Latitude,
                LowSignalLongitude = record.Longitude,
                HighSignalLevel = record.Rssi,
                HighSignalLatitude = record.Latitude,
                HighSignalLongitude = record.Longitude,
                RawSecurityPayload = record.AuthMode,
                UserId = ScopeWrapperService.GetUserId(),
                ScanDate = record.FirstSeen
            });

            var manufacturer = await OuiLookupService.GetManufacturerName(accessPoint.Bssid);

            accessPoint.Apply(new Events.V1.AccessPointManufacturerChanged
            {
                Id = accessPoint.Id,
                Manufacturer = manufacturer
            });

            accessPoint.Apply(new Events.V1.AccessPointAdnnotationCreated
            {
                Id = accessPoint.Id,
                Title = _adnnotationName,
                Content = SerializeRawAccessPointRecord(record)
            });

            await UnitOfWork.AccessPointRepository.Add(accessPoint);
        }

        private async Task CreateAccessPointStamp(AccessPointRecord record)
        {
            var accessPoint = await UnitOfWork.AccessPointRepository.Get(record.Mac);

            accessPoint.Apply(new Events.V1.AccessPointStampCreated
            {
                Id = accessPoint.Id,
                Ssid = record.Ssid,
                Frequency = _defaultFrequencyValue,
                LowSignalLevel = record.Rssi,
                LowSignalLatitude = record.Latitude,
                LowSignalLongitude = record.Longitude,
                HighSignalLevel = record.Rssi,
                HighSignalLatitude = record.Latitude,
                HighSignalLongitude = record.Longitude,
                RawSecurityPayload = record.AuthMode,
                UserId = ScopeWrapperService.GetUserId(),
                ScanDate = record.FirstSeen
            });

            accessPoint.Apply(new Events.V1.AccessPointAdnnotationCreated
            {
                Id = accessPoint.Id,
                Title = _adnnotationName,
                Content = SerializeRawAccessPointRecord(record)
            });
        }

        private static AccessPointRecord CombineRecords(IEnumerable<AccessPointRecord> records)
        {
            var accessPoint = records.First();

            foreach(var record in records)
            {
                if (accessPoint.Rssi < record.Rssi)
                {
                    accessPoint.Rssi = record.Rssi;
                    accessPoint.Latitude = record.Latitude;
                    accessPoint.Longitude = record.Longitude;
                }

                if (accessPoint.LowSignalLevel > record.LowSignalLevel)
                {
                    accessPoint.LowSignalLevel = record.LowSignalLevel;
                    accessPoint.LowLatitude = record.LowLatitude;
                    accessPoint.LowLongitude = record.LowLongitude;
                }

                if (record.FirstSeen > accessPoint.FirstSeen)
                {
                    accessPoint.FirstSeen = record.FirstSeen;
                }
            }

            return accessPoint;
        }

        private static AccessPointRecord AccessPointToRecord(AccessPoint accessPoint)
        {
            return new AccessPointRecord
            {
                Mac = accessPoint.Bssid.Value,
                Ssid = accessPoint.Ssid.Value,
                AuthMode = accessPoint.Security.RawSecurityPayload,
                FirstSeen = accessPoint.CreationTimestamp.Value,
                Channel = 0,
                Rssi = accessPoint.Positioning.HighSignalLevel,
                Latitude = accessPoint.Positioning.HighSignalLatitude,
                Longitude = accessPoint.Positioning.HighSignalLongitude,
                Altituded = 0,
                Accuracy = 0,
                Type = "WIFI"
            };
        }

        private static string SerializeRawAccessPointRecord(AccessPointRecord record)
        {
            return JsonSerializer.Serialize(record, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
