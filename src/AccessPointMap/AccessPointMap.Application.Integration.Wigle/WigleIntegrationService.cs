using AccessPointMap.Application.Core;
using AccessPointMap.Application.Integration.Core;
using AccessPointMap.Application.Integration.Core.Exceptions;
using AccessPointMap.Application.Integration.Wigle.Extensions;
using AccessPointMap.Application.Integration.Wigle.Models;
using AccessPointMap.Application.Oui.Core;
using AccessPointMap.Application.Pcap.Core;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Domain.Core.Exceptions;
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
using System.Threading;
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
            IScopeWrapperService scopeWrapperService,
            IPcapParsingService pcapParsingService,
            IOuiLookupService ouiLookupService) : base(unitOfWork, scopeWrapperService, pcapParsingService, ouiLookupService) { }

        public async Task<Result> HandleCommandAsync(IIntegrationCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                return command switch
                {
                    Commands.CreateAccessPointsFromCsvFile cmd => await HandleCommand(cmd, cancellationToken),
                    Commands.CreateAccessPointsFromCsvGzFile cmd => await HandleCommand(cmd, cancellationToken),
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
            catch (TaskCanceledException)
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
                    Queries.ExportAccessPointsToCsv q => await HandleQuery(q, cancellationToken),
                    _ => throw new IntegrationException($"This query is not supported by the {IntegrationName} integration."),
                };
            }
            catch (DomainException ex)
            {
                return Result.Failure<object>(IntegrationError.FromDomainException(ex));
            }
            catch (IntegrationException ex)
            {
                return Result.Failure<object>(IntegrationError.FromIntegrationException(ex));
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch
            {
                throw;
            }
        }

        private async Task<Result> HandleCommand(Commands.CreateAccessPointsFromCsvFile cmd, CancellationToken cancellationToken = default)
        {
            if (cmd.ScanCsvFile is null)
                return Result.Failure(WigleIntegrationError.UploadedCsvFileIsNull);

            if (Path.GetExtension(cmd.ScanCsvFile.FileName).ToLower() != ".csv")
                return Result.Failure(WigleIntegrationError.UploadedFileHasInvalidFormat);

            using var sr = new StreamReader(cmd.ScanCsvFile.OpenReadStream());
            sr.SkipLine();

            using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);

            cancellationToken.ThrowIfCancellationRequested();

            var records = csv.GetRecords<AccessPointRecord>()
                .GroupBy(r => r.Mac, (k, v) => CombineRecords(v))
                .ToList();

            await HandleAccessPointRecords(records, cancellationToken);
            
            return Result.Success();
        }

        private async Task<Result> HandleCommand(Commands.CreateAccessPointsFromCsvGzFile cmd, CancellationToken cancellationToken = default)
        {
            if (cmd.ScanCsvGzFile is null)
                return Result.Failure(WigleIntegrationError.UploadedCsvGzFileIsNull);

            if (!cmd.ScanCsvGzFile.FileName.ToLower().EndsWith(".csv.gz"))
                return Result.Failure(WigleIntegrationError.UploadedFileHasInvalidFormat);

            using var compressionStream = new GZipStream(cmd.ScanCsvGzFile.OpenReadStream(), CompressionMode.Decompress);
            using var sr = new StreamReader(compressionStream);
            sr.SkipLine();

            using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);

            cancellationToken.ThrowIfCancellationRequested();

            var records = csv.GetRecords<AccessPointRecord>()
                .GroupBy(r => r.Mac, (k, v) => CombineRecords(v))
                .ToList();

            await HandleAccessPointRecords(records, cancellationToken);

            return Result.Success();
        }

        private async Task HandleAccessPointRecords(IEnumerable<AccessPointRecord> accessPointRecords, CancellationToken cancellationToken = default)
        {
            var filteredRecords = accessPointRecords
                .Where(r => r.Type.ToUpper().Contains(_allowedType));

            var runRecordGroups = GroupAccessPointsByRun(filteredRecords);

            foreach (var runGroup in runRecordGroups)
            {
                var runIdentifier = runGroup.Key;

                foreach (var record in runGroup.Value)
                {
                    if (await UnitOfWork.AccessPointRepository.ExistsAsync(record.Mac, cancellationToken))
                    {
                        await CreateAccessPointStamp(record, runIdentifier, cancellationToken);
                        continue;
                    }

                    await CreateAccessPoint(record, runIdentifier, cancellationToken);
                }
            }

            await UnitOfWork.Commit(cancellationToken);
        }

        private async Task<Result<object>> HandleQuery(Queries.ExportAccessPointsToCsv _, CancellationToken cancellationToken = default)
        {
            // TODO: Prepare specific query
            var accessPoints = UnitOfWork.AccessPointRepository.Entities
                .Where(a => !a.DeletedAt.HasValue)
                .Where(a => a.DisplayStatus.Value)
                .ToList();

            var records = accessPoints
                .Select(a => AccessPointToRecord(a));

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    foreach (var preheaderField in _csvPreheader.Split(','))
                    {
                        csvWriter.WriteField(preheaderField);
                    }

                    await csvWriter.NextRecordAsync();

                    await csvWriter.WriteRecordsAsync(records);
                }

                var exportFile = ExportFile.FromBuffer(memoryStream.ToArray());
                return Result.Success<object>(exportFile);
            }
        }

        private async Task CreateAccessPoint(AccessPointRecord record, Guid? runIdentifier, CancellationToken cancellationToken = default)
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

            var ouiLookupResult = await OuiLookupService.GetManufacturerNameAsync(accessPoint.Bssid, cancellationToken);

            if (ouiLookupResult.IsSuccess)
            {
                accessPoint.Apply(new Events.V1.AccessPointManufacturerChanged
                {
                    Id = accessPoint.Id,
                    Manufacturer = ouiLookupResult.Value
                });
            }

            accessPoint.Apply(new Events.V1.AccessPointAdnnotationCreated
            {
                Id = accessPoint.Id,
                Title = _adnnotationName,
                Content = SerializeRawAccessPointRecord(record)
            });

            await UnitOfWork.AccessPointRepository.AddAsync(accessPoint, cancellationToken);
        }

        private async Task CreateAccessPointStamp(AccessPointRecord record, Guid? runIdentifier, CancellationToken cancellationToken = default)
        {
            var accessPoint = await UnitOfWork.AccessPointRepository.GetAsync(record.Mac, cancellationToken);

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
