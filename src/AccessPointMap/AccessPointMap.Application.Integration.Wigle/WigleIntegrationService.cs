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
using System.Linq;
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
        private const string _integrationVersion = "1.0.0";

        private const double _defaultFrequencyValue = default;

        protected override string IntegrationName => _integrationName;
        protected override string IntegrationDescription => _integrationDescription;
        protected override string IntegrationVersion => _integrationVersion;

        public WigleIntegrationService(
            IUnitOfWork unitOfWork,
            IScopeWrapperService scopeWrapperService,
            IPcapParsingService pcapParsingService,
            IOuiLookupService ouiLookupService) : base(unitOfWork, scopeWrapperService, pcapParsingService, ouiLookupService) { }

        public async Task Handle(IIntegrationCommand command)
        {
            switch (command)
            {
                case Commands.CreateAccessPointsFromCsvFile cmd: await HandleCommand(cmd); break;

                default: throw new IntegrationException($"This command is not supported by the {IntegrationName} integration.");
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

            Guid? runIdentifier = IsSingleRun(records)
                ? Guid.NewGuid()
                : null;

            foreach (var record in records)
            {
                if (!record.Type.ToUpper().Contains(_allowedType)) continue;

                if (await UnitOfWork.AccessPointRepository.Exists(record.Mac))
                {
                    await CreateAccessPointStamp(record, runIdentifier);
                    continue;
                }

                await CreateAccessPoint(record, runIdentifier);
            }

            await UnitOfWork.Commit();
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

        private async Task CreateAccessPointStamp(AccessPointRecord record, Guid? runIdentifier)
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

        private static bool IsSingleRun(IEnumerable<AccessPointRecord> records)
        {
            if (records.Count() < 2) return false;

            var firstRecordDate = records.Min(r => r.FirstSeen);
            var lastRecordData = records.Max(r => r.FirstSeen);

            const double hoursThreshold = 18;
            var hoursDifference = (lastRecordData - firstRecordDate).TotalHours;

            return hoursDifference < hoursThreshold;
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

        private static string SerializeRawAccessPointRecord(AccessPointRecord record)
        {
            return JsonSerializer.Serialize(record, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
