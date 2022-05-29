using AccessPointMap.Application.Integration.Aircrackng.Models;
using AccessPointMap.Application.Integration.Core;
using AccessPointMap.Application.Integration.Core.Exceptions;
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

namespace AccessPointMap.Application.Integration.Aircrackng
{
    public class AircrackngIntegrationService : AccessPointIntegrationBase<AircrackngIntegrationService>, IAircrackngIntegrationService
    {
        private readonly string[] _allowedExtensions = new string[] { ".csv" };
        private readonly string _allowedType = "AP";

        private readonly string _adnnotationName = "Aircrack-ng integration provided data";

        private const string _integrationName = "Aircrack-ng";
        private const string _integrationDescription = "Integration for the popular WiFi security auditing tools suite.";
        private const string _integrationVersion = "0.0.0-alpha";

        private const double _defaultFrequencyValue = default;

        protected override string IntegrationName => _integrationName;
        protected override string IntegrationDescription => _integrationDescription;
        protected override string IntegrationVersion => _integrationVersion;

        public AircrackngIntegrationService(IUnitOfWork unitOfWork, IScopeWrapperService scopeWrapperService) : base(unitOfWork, scopeWrapperService) { }

        public async Task Create(Requests.Create request)
        {
            ValidateCsvDumpFile(request.CsvDumpFile);

            try
            {
                await HandleCreate(request);
            }
            catch (Exception ex)
            {
                throw new AccessPointIntegrationException("Aircrack-ng integration service failed while parsing provied data.", ex);
            }
            
        }

        public async Task HandleCreate(Requests.Create request)
        {
            var accessPoints = ParseAccessPointDumps(request.CsvDumpFile);

            foreach (var accessPoint in accessPoints)
            {
                if (await _unitOfWork.AccessPointRepository.Exists(accessPoint.Bssid))
                {
                    await CreateAccessPointStamp(accessPoint);
                    continue;
                }

                await CreateAccessPoint(accessPoint);
            }

            await _unitOfWork.Commit();
        }

        private void ValidateCsvDumpFile(IFormFile csv)
        {
            if (csv is null)
                throw new ArgumentNullException(nameof(csv));

            var extension = Path.GetExtension(csv.FileName);

            if (!_allowedExtensions.Contains(extension.ToLower()))
                throw new ArgumentNullException(nameof(csv));
        }

        private IEnumerable<AccessPointRecord> ParseAccessPointDumps(IFormFile csvFile)
        {
            using var sr = new StreamReader(csvFile.OpenReadStream());

            using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);

            var accessPoints = new Dictionary<string, AccessPointRecord>();

            foreach (var record in csv.GetRecords<AccessPointRecord>())
            {
                if (!record.Type.ToUpper().Contains(_allowedType)) continue;

                if (!accessPoints.ContainsKey(record.Bssid))
                {
                    accessPoints.Add(record.Bssid, record);
                    continue;
                }

                var accessPoint = accessPoints[record.Bssid];

                if (record.Power < accessPoint.Power)
                {
                    accessPoint.Power = record.Power;
                    accessPoint.Latitude = record.Latitude;
                    accessPoint.Latitude = record.Longitude;
                }

                if (record.LowSignalLevel > accessPoint.LowSignalLevel)
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

        private async Task CreateAccessPoint(AccessPointRecord record)
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
                UserId = _scopeWrapperService.GetUserId(),
                ScanDate = record.LocalTimestamp
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
            var accessPoint = await _unitOfWork.AccessPointRepository.Get(record.Bssid);

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
                UserId = _scopeWrapperService.GetUserId(),
                ScanDate = record.LocalTimestamp
            });

            accessPoint.Apply(new Events.V1.AccessPointAdnnotationCreated
            {
                Id = accessPoint.Id,
                Title = _adnnotationName,
                Content = SerializeRawAccessPointRecord(record)
            });
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
