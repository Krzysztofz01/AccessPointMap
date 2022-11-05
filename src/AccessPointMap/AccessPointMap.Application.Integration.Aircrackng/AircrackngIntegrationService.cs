using AccessPointMap.Application.Core;
using AccessPointMap.Application.Integration.Aircrackng.Models;
using AccessPointMap.Application.Integration.Core;
using AccessPointMap.Application.Integration.Core.Exceptions;
using AccessPointMap.Application.Integration.Core.Extensions;
using AccessPointMap.Application.Logging;
using AccessPointMap.Application.Oui.Core;
using AccessPointMap.Application.Pcap.Core;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Infrastructure.Core.Abstraction;
using CsvHelper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Integration.Aircrackng
{
    public class AircrackngIntegrationService : AccessPointIntegrationBase<AircrackngIntegrationService>, IAircrackngIntegrationService
    {
        private readonly string _adnnotationName = "Aircrack-ng integration provided data";

        private const string _integrationName = "Aircrack-ng";
        private const string _integrationDescription = "Integration for the popular WiFi security auditing tools suite.";
        private const string _integrationVersion = "1.1.0";

        private const double _defaultFrequencyValue = default;

        protected override string IntegrationName => _integrationName;
        protected override string IntegrationDescription => _integrationDescription;
        protected override string IntegrationVersion => _integrationVersion;

        public AircrackngIntegrationService(
            IUnitOfWork unitOfWork,
            IScopeWrapperService scopeWrapperService,
            IPcapParsingService pcapParsingService,
            IOuiLookupService ouiLookupService,
            ILogger<AircrackngIntegrationService> logger) : base(unitOfWork, scopeWrapperService, pcapParsingService, ouiLookupService, logger) { }

        public async Task<Result> HandleCommandAsync(IIntegrationCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                return command switch
                {
                    Commands.CreateAccessPointsFromCsvFile cmd => await HandleCommand(cmd, cancellationToken),
                    Commands.CreatePacketsFromPcapFile cmd => await HandleCommand(cmd, cancellationToken),
                    _ => throw new IntegrationException($"This command is not supported by the {IntegrationName} integration."),
                };
            }
            catch (DomainException ex)
            {
                return Result.Failure(IntegrationError.FromDomainException(ex));
            }
            catch (IntegrationException ex)
            {
                return Result.Failure(IntegrationError.FromIntegrationException(ex));
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Result<object>> HandleQueryAsync(IIntegrationQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                return query switch
                {
                    _ => throw new IntegrationException($"This query is not supported by the {IntegrationName} integration.")
                };
            }
            catch (DomainException ex)
            {
                return await Task.FromResult(Result.Failure<object>(IntegrationError.FromDomainException(ex)));
            }
            catch (IntegrationException ex)
            {
                return await Task.FromResult(Result.Failure<object>(IntegrationError.FromIntegrationException(ex)));
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch
            {
                throw;
            }
        }

        private async Task<Result> HandleCommand(Commands.CreatePacketsFromPcapFile cmd, CancellationToken cancellationToken = default)
        {
            if (cmd.ScanPcapFile is null)
                return Result.Failure(AircrackngIntegrationError.UploadedPcapFileIsNull);

            if (Path.GetExtension(cmd.ScanPcapFile.FileName).ToLower() != ".cap")
                return Result.Failure(AircrackngIntegrationError.UploadedFileHasInvalidFormat);

            var packetMapResult = await PcapParsingService.MapPacketsToMacAddressesAsync(cmd.ScanPcapFile, cancellationToken);

            if (packetMapResult.IsFailure) return Result.Failure(packetMapResult.Error);

            foreach (var map in packetMapResult.Value)
            {
                await CreateAccessPointPackets(map.Key, map.Value, cancellationToken);
            }

            await UnitOfWork.Commit(cancellationToken);

            return Result.Success();
        }

        private async Task<Result> HandleCommand(Commands.CreateAccessPointsFromCsvFile cmd, CancellationToken cancellationToken = default)
        {
            if (cmd.ScanCsvFile is null)
                return Result.Failure(AircrackngIntegrationError.UploadedCsvFileIsNull);

            if (Path.GetExtension(cmd.ScanCsvFile.FileName).ToLower() != ".csv")
                return Result.Failure(AircrackngIntegrationError.UploadedFileHasInvalidFormat);

            var accessPoints = ParseCsvAccessPointScanFile(cmd.ScanCsvFile.OpenReadStream(), cancellationToken);
            var runRecordGroups = GroupAccessPointsByRun(accessPoints);

            foreach (var runGroup in runRecordGroups)
            {
                var runIdentifier = runGroup.Key;

                foreach (var record in runGroup.Value)
                {
                    if (await UnitOfWork.AccessPointRepository.ExistsAsync(record.Bssid, cancellationToken))
                    {
                        await CreateAccessPointStamp(record, runIdentifier, cancellationToken);
                        continue;
                    }

                    await CreateAccessPoint(record, runIdentifier, cancellationToken);
                }
            }

            await UnitOfWork.Commit(cancellationToken);

            return Result.Success();
        }

        private async Task CreateAccessPoint(AccessPointRecord record, Guid? runIdentifier, CancellationToken cancellationToken = default)
        {
            var @event = new Events.V1.AccessPointCreated
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
            };

            Logger.LogDomainCreationEvent(@event);

            var accessPoint = AccessPoint.Factory.Create(@event);

            var ouiLookupResult = await OuiLookupService.GetManufacturerNameAsync(accessPoint.Bssid, cancellationToken);

            if (ouiLookupResult.IsSuccess)
            {
                accessPoint.ApplyWithLogging(new Events.V1.AccessPointManufacturerChanged
                {
                    Id = accessPoint.Id,
                    Manufacturer = ouiLookupResult.Value
                }, Logger);
            }

            accessPoint.ApplyWithLogging(new Events.V1.AccessPointAdnnotationCreated
            {
                Id = accessPoint.Id,
                Title = _adnnotationName,
                Content = SerializeRawAccessPointRecord(record)
            }, Logger);

            await UnitOfWork.AccessPointRepository.AddAsync(accessPoint, cancellationToken);
        }

        private async Task CreateAccessPointStamp(AccessPointRecord record, Guid? runIdentifier, CancellationToken cancellationToken = default)
        {
            var accessPoint = await UnitOfWork.AccessPointRepository.GetAsync(record.Bssid, cancellationToken);

            accessPoint.ApplyWithLogging(new Events.V1.AccessPointStampCreated
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
            }, Logger);

            accessPoint.ApplyWithLogging(new Events.V1.AccessPointAdnnotationCreated
            {
                Id = accessPoint.Id,
                Title = _adnnotationName,
                Content = SerializeRawAccessPointRecord(record)
            }, Logger);
        }

        private async Task CreateAccessPointPackets(string bssid, IEnumerable<Packet> packets, CancellationToken cancellationToken = default)
        {
            if (!await UnitOfWork.AccessPointRepository.ExistsAsync(bssid, cancellationToken)) return;

            var accessPoint = await UnitOfWork.AccessPointRepository.GetAsync(bssid, cancellationToken);

            foreach (var packet in packets)
            {
                cancellationToken.ThrowIfCancellationRequested();

                accessPoint.ApplyWithLogging(new Events.V1.AccessPointPacketCreated
                {
                    Id = accessPoint.Id,
                    SourceAddress = packet.SourceAddress,
                    DestinationAddress = packet.DestinationAddress,
                    FrameType = packet.FrameType,
                    Data = packet.Data
                }, Logger);
            }

            accessPoint.ApplyWithLogging(new Events.V1.AccessPointAdnnotationCreated
            {
                Id = accessPoint.Id,
                Title = _adnnotationName,
                Content = $"Inserted {packets.Count()} IEEE 802.11 frames."
            }, Logger);
        }

        private static IDictionary<Guid, IList<AccessPointRecord>> GroupAccessPointsByRun(IEnumerable<AccessPointRecord> records)
        {
            var accessPointRunGrouping = new Dictionary<Guid, IList<AccessPointRecord>>();

            const double minutesThreshold = 30;
            var currentRun = Guid.NewGuid();

            foreach (var accessPoint in records.OrderBy(r => r.LocalTimestamp))
            {
                if (accessPointRunGrouping.Count == 0)
                {
                    accessPointRunGrouping.Add(currentRun, new List<AccessPointRecord>() { accessPoint });
                    continue;
                }

                var lastRunRecord = accessPointRunGrouping[currentRun].Last();

                var timeDifference = (accessPoint.LocalTimestamp - lastRunRecord.LocalTimestamp).TotalMinutes;
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

        private static string SerializeRawAccessPointRecord(AccessPointRecord record)
        {
            return JsonSerializer.Serialize(record, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        private static IEnumerable<AccessPointRecord> ParseCsvAccessPointScanFile(Stream csvFileStream, CancellationToken cancellationToken = default)
        {
            const string _allowedType = "AP";

            using var sr = new StreamReader(csvFileStream);

            using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);

            var accessPoints = new Dictionary<string, AccessPointRecord>();

            while (csv.Read())
            {
                cancellationToken.ThrowIfCancellationRequested();

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
