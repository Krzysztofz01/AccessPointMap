using AccessPointMap.Application.Integration.Core;
using AccessPointMap.Application.Integration.Core.Exceptions;
using AccessPointMap.Application.Integration.Wigle.Models;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Infrastructure.Core.Abstraction;
using CsvHelper;
using Microsoft.AspNetCore.Http;
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
        private readonly string[] _allowedExtensions = new string[] { ".csv" };
        private readonly string _allowedType = "WIFI";

        private readonly string _adnnotationName = "WiGLE integration provided data";

        private const string _integrationName = "WiGLE";
        private const string _integrationDescription = "Integration for the bigest wardriving platform and their scanning application";
        private const string _integrationVersion = "0.0.0-alpha";

        private const double _defaultFrequencyValue = default;

        protected override string IntegrationName => _integrationName;
        protected override string IntegrationDescription => _integrationDescription;
        protected override string IntegrationVersion => _integrationVersion;

        public WigleIntegrationService(IUnitOfWork unitOfWork, IScopeWrapperService scopeWrapperService) : base(unitOfWork, scopeWrapperService) { }

        public async Task Create(Requests.Create request)
        {
            ValidateCsvDatabaseFile(request.CsvDatabaseFile);

            try
            {
                await HandleCreate(request);
            }
            catch (Exception ex)
            {
                throw new AccessPointIntegrationException("Wigle integration service failed while parsing provied data.", ex);
            }
        }

        private async Task HandleCreate(Requests.Create request)
        {
            using var sr = new StreamReader(request.CsvDatabaseFile.OpenReadStream());

            // This line will shift the stream by one line to avoid the header
            sr.ReadLine();

            using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<AccessPointRecord>()
                .GroupBy(r => r.Mac, (k, v) => CombineRecords(v))
                .ToList();

            foreach (var record in records)
            {
                if (!record.Type.ToUpper().Contains(_allowedType)) continue;

                if (await _unitOfWork.AccessPointRepository.Exists(record.Mac))
                {
                    await CreateAccessPointStamp(record);
                    continue;
                }

                await CreateAccessPoint(record);
            }

            await _unitOfWork.Commit();
        }

        private void ValidateCsvDatabaseFile(IFormFile csv)
        {
            if (csv is null)
                throw new ArgumentNullException(nameof(csv));

            var extension = Path.GetExtension(csv.FileName);

            if (!_allowedExtensions.Contains(extension.ToLower()))
                throw new ArgumentNullException(nameof(csv));
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
                UserId = _scopeWrapperService.GetUserId(),
                ScanDate = record.FirstSeen
            });

            accessPoint.Apply(new Events.V1.AccessPointAdnnotationCreated
            {
                Id = accessPoint.Id,
                Title = _adnnotationName,
                Content = SerializeRawAccessPointRecord(record)
            });

            await _unitOfWork.AccessPointRepository.Add(accessPoint);
        }

        private async Task CreateAccessPointStamp(AccessPointRecord record)
        {
            var accessPoint = await _unitOfWork.AccessPointRepository.Get(record.Mac);

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
                UserId = _scopeWrapperService.GetUserId(),
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

        private string SerializeRawAccessPointRecord(AccessPointRecord record)
        {
            return JsonSerializer.Serialize(record, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
