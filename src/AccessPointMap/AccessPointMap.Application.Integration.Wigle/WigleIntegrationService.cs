using AccessPointMap.Application.Integration.Core;
using AccessPointMap.Application.Integration.Core.Exceptions;
using AccessPointMap.Application.Integration.Wigle.Extensions;
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

        private readonly string _csvPreheader = $"WigleWifi-1.4,appRelease=${_integrationVersion},model=AccessPointMap,release=AccessPointMap,device=AccessPointMap,display=AccessPointMap,board=AccessPointMap,brand=AccessPointMap";

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
            sr.SkipLine();

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
            sr.SkipLine();

            using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<AccessPointRecord>()
                .GroupBy(r => r.Mac, (k, v) => CombineRecords(v))
                .ToList();

            await HandleAccessPointRecords(records);
        }

        private async Task HandleAccessPointRecords(IEnumerable<AccessPointRecord> accessPointRecords)
        {
            var filteredRecords = accessPointRecords
                .Where(r => r.Type.ToUpper().Contains(_allowedType));

            var runRecordGroups = GroupAccessPointsByRun(filteredRecords);

            foreach (var runGroup in runRecordGroups)
            {
                var runIdentifier = runGroup.Key;

                foreach (var record in runGroup.Value)
                {
                    // TODO: Pass the CancellationToken to the repository method
                    if (await UnitOfWork.AccessPointRepository.ExistsAsync(record.Mac))
                    {
                        await CreateAccessPointStamp(record, runIdentifier);
                        continue;
                    }

                    await CreateAccessPoint(record, runIdentifier);
                }
            }

            await UnitOfWork.Commit();
        }

        private async Task<object> HandleQuery(Queries.ExportAccessPointsToCsv _)
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

        private async Task CreateAccessPoint(AccessPointRecord record, Guid? runIdentifier)
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
                ScanDate = record.FirstSeen,
                RunIdentifier = runIdentifier
            });

            // TODO: Pass CancellationToken to the method
            var manufacturer = await OuiLookupService.GetManufacturerNameAsync(accessPoint.Bssid);

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

            // TODO: Pass the CancellationToken to the repository method
            await UnitOfWork.AccessPointRepository.AddAsync(accessPoint);
        }

        private async Task CreateAccessPointStamp(AccessPointRecord record, Guid? runIdentifier)
        {
            // TODO: Pass the CancellationToken to the repository method
            var accessPoint = await UnitOfWork.AccessPointRepository.GetAsync(record.Mac);

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
                ScanDate = record.FirstSeen,
                RunIdentifier = runIdentifier
            });

            accessPoint.Apply(new Events.V1.AccessPointAdnnotationCreated
            {
                Id = accessPoint.Id,
                Title = _adnnotationName,
                Content = SerializeRawAccessPointRecord(record)
            });
        }


        private static IDictionary<Guid, IList<AccessPointRecord>> GroupAccessPointsByRun(IEnumerable<AccessPointRecord> records)
        {
            var accessPointRunGrouping = new Dictionary<Guid, IList<AccessPointRecord>>();

            const double minutesThreshold = 30;
            var currentRun = Guid.NewGuid();

            foreach (var accessPoint in records.OrderBy(r => r.FirstSeen))
            {
                if (accessPointRunGrouping.Count == 0)
                {
                    accessPointRunGrouping.Add(currentRun, new List<AccessPointRecord>() { accessPoint });
                    continue;
                }

                var lastRunRecord = accessPointRunGrouping[currentRun].Last();
  
                var timeDifference = (accessPoint.FirstSeen - lastRunRecord.FirstSeen).TotalMinutes;
                if (timeDifference < minutesThreshold)
                {
                    accessPointRunGrouping[currentRun].Add(accessPoint);
                    continue;
                }

                currentRun = Guid.NewGuid();
                accessPointRunGrouping.Add(currentRun, new List<AccessPointRecord>() { accessPoint });
            }

            return accessPointRunGrouping;
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
