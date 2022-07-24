using AccessPointMap.Application.Integration.Aircrackng.Models;
using AccessPointMap.Application.Integration.Core;
using AccessPointMap.Application.Integration.Core.Exceptions;
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

namespace AccessPointMap.Application.Integration.Aircrackng
{
    public class AircrackngIntegrationService : AccessPointIntegrationBase<AircrackngIntegrationService>, IAircrackngIntegrationService
    {
        private readonly string _adnnotationName = "Aircrack-ng integration provided data";

        private const string _integrationName = "Aircrack-ng";
        private const string _integrationDescription = "Integration for the popular WiFi security auditing tools suite.";
        private const string _integrationVersion = "1.0.0";

        private const double _defaultFrequencyValue = default;

        protected override string IntegrationName => _integrationName;
        protected override string IntegrationDescription => _integrationDescription;
        protected override string IntegrationVersion => _integrationVersion;

        public AircrackngIntegrationService(
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
                case Commands.CreatePacketsFromPcapFile cmd: await HandleCommand(cmd); break;

                default: throw new IntegrationException($"This command is not supported by the {IntegrationName} integration.");
            }
        }

        public Task<object> Query(IIntegrationQuery query)
        {
            switch (query)
            {
                default: throw new IntegrationException($"This query is not supported by the {IntegrationName} integration.");
            }
        }

        private async Task HandleCommand(Commands.CreatePacketsFromPcapFile cmd)
        {
            if (cmd.ScanPcapFile is null)
                throw new ArgumentNullException(nameof(cmd));

            if (Path.GetExtension(cmd.ScanPcapFile.FileName).ToLower() != ".cap")
                throw new ArgumentNullException(nameof(cmd));

            var packetMap = await PcapParsingService.MapPacketsToMacAddresses(cmd.ScanPcapFile);

            foreach (var map in packetMap)
            {
                await CreateAccessPointPackets(map.Key, map.Value);
            }

            await UnitOfWork.Commit();
        }

        private async Task HandleCommand(Commands.CreateAccessPointsFromCsvFile cmd)
        {
            if (cmd.ScanCsvFile is null)
                throw new ArgumentNullException(nameof(cmd));

            if (Path.GetExtension(cmd.ScanCsvFile.FileName).ToLower() != ".csv")
                throw new ArgumentNullException(nameof(cmd));

            var accessPoints = ParseCsvAccessPointScanFile(cmd.ScanCsvFile.OpenReadStream());

            Guid? runIdentifier = IsSingleRun(accessPoints)
                ? Guid.NewGuid()
                : null;

            foreach (var accessPoint in accessPoints)
            {
                if (await UnitOfWork.AccessPointRepository.Exists(accessPoint.Bssid))
                {
                    await CreateAccessPointStamp(accessPoint, runIdentifier);
                    continue;
                }

                await CreateAccessPoint(accessPoint, runIdentifier);
            }

            await UnitOfWork.Commit();
        }

        private async Task CreateAccessPoint(AccessPointRecord record, Guid? runIdentifier)
        {
            var accessPoint = AccessPoint.Factory.Create(new Events.V1.AccessPointCreated
            {
                Bssid = record.Bssid,
                Ssid = record.Ssid,
                Frequency = _defaultFrequencyValue,
                LowSignalLevel = record.LowSignalLevel,
                LowSignalLatitude = record.LowLatitude,
                LowSignalLongitude = record.LowLongitude,
                HighSignalLevel = record.Power,
                HighSignalLatitude = record.Latitude,
                HighSignalLongitude = record.Longitude,
                RawSecurityPayload = record.Security,
                UserId = ScopeWrapperService.GetUserId(),
                ScanDate = record.LocalTimestamp,
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
            var accessPoint = await UnitOfWork.AccessPointRepository.Get(record.Bssid);

            accessPoint.Apply(new Events.V1.AccessPointStampCreated
            {
                Id = accessPoint.Id,
                Ssid = record.Ssid,
                Frequency = _defaultFrequencyValue,
                LowSignalLevel = record.LowSignalLevel,
                LowSignalLatitude = record.LowLatitude,
                LowSignalLongitude = record.LowLongitude,
                HighSignalLevel = record.Power,
                HighSignalLatitude = record.Latitude,
                HighSignalLongitude = record.Longitude,
                RawSecurityPayload = record.Security,
                UserId = ScopeWrapperService.GetUserId(),
                ScanDate = record.LocalTimestamp,
                RunIdentifier = runIdentifier
            });

            accessPoint.Apply(new Events.V1.AccessPointAdnnotationCreated
            {
                Id = accessPoint.Id,
                Title = _adnnotationName,
                Content = SerializeRawAccessPointRecord(record)
            });
        }

        private async Task CreateAccessPointPackets(string bssid, IEnumerable<Packet> packets)
        {
            if (!await UnitOfWork.AccessPointRepository.Exists(bssid)) return;
            
            var accessPoint = await UnitOfWork.AccessPointRepository.Get(bssid);

            foreach (var packet in packets)
            {
                accessPoint.Apply(new Events.V1.AccessPointPacketCreated
                {
                    Id = accessPoint.Id,
                    SourceAddress = packet.SourceAddress,
                    DestinationAddress = packet.DestinationAddress,
                    FrameType = packet.FrameType,
                    Data = packet.Data
                });
            }

            accessPoint.Apply(new Events.V1.AccessPointAdnnotationCreated
            {
                Id = accessPoint.Id,
                Title = _adnnotationName,
                Content = $"Inserted {packets.Count()} IEEE 802.11 frames."
            });
        }

        private static bool IsSingleRun(IEnumerable<AccessPointRecord> records)
        {
            if (records.Count() < 2) return false;

            var firstRecordDate = records.Min(r => r.LocalTimestamp);
            var lastRecordData = records.Max(r => r.LocalTimestamp);

            const double hoursThreshold = 18;
            var hoursDifference = (lastRecordData - firstRecordDate).TotalHours;

            return hoursDifference < hoursThreshold;
        }

        private static string SerializeRawAccessPointRecord(AccessPointRecord record)
        {
            return JsonSerializer.Serialize(record, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        private static IEnumerable<AccessPointRecord> ParseCsvAccessPointScanFile(Stream csvFileStream)
        {
            const string _allowedType = "AP";

            using var sr = new StreamReader(csvFileStream);

            using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);

            var accessPoints = new Dictionary<string, AccessPointRecord>();

            while (csv.Read())
            {
                // TODO: Some SSID'S are containing comma's which are confusing the CsvHelper parser
                // The current solution is to skip all invalid rows.
                AccessPointRecord record = null;
                try
                {
                    record = csv.GetRecord<AccessPointRecord>();
                }
                catch (Exception)
                {
                }

                if (record is null) continue;

                if (!record.Type.ToUpper().Contains(_allowedType)) continue;

                if (!accessPoints.ContainsKey(record.Bssid))
                {
                    accessPoints.Add(record.Bssid, record);
                    continue;
                }

                var accessPoint = accessPoints[record.Bssid];

                if (record.Power > accessPoint.Power)
                {
                    accessPoint.Power = record.Power;
                    accessPoint.Latitude = record.Latitude;
                    accessPoint.Latitude = record.Longitude;
                }

                if (record.LowSignalLevel < accessPoint.LowSignalLevel)
                {
                    accessPoint.LowSignalLevel = record.LowSignalLevel;
                    accessPoint.LowLatitude = record.LowLatitude;
                    accessPoint.LowLongitude = record.LowLongitude;
                }

                if (record.LocalTimestamp > accessPoint.LocalTimestamp)
                {
                    accessPoint.LocalTimestamp = record.LocalTimestamp;
                }

                accessPoints[record.Bssid] = accessPoint;
            }

            return accessPoints.Select(a => a.Value);
        }
    }
}
