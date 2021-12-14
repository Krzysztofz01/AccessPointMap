using AccessPointMap.Application.Integration.Aircrackng.Models;
using AccessPointMap.Application.Integration.Core;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Infrastructure.Core.Abstraction;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Integration.Aircrackng
{
    public class AircrackngIntegrationService : AccessPointIntegrationBase<AircrackngIntegrationService>, IAircrackngIntegrationService
    {
        private readonly string[] _allowedExtensions = new string[] { ".csv" };

        private const string _integrationName = "Aircrack-ng";
        private const string _integrationDescription = "Integration for the popular WiFi security auditing tools suite.";
        private const string _integrationVersion = "0.0.0-alpha";

        private const double _defaultFrequencyValue = default;

        protected override string IntegrationName => _integrationName;
        protected override string IntegrationDescription => _integrationDescription;
        protected override string IntegrationVersion => _integrationVersion;

        public AircrackngIntegrationService(IUnitOfWork unitOfWork, IScopeWrapperService scopeWrapperService) : base(unitOfWork, scopeWrapperService) { }

        private void ValidateCsvDumpFile(IFormFile csv)
        {
            if (csv is null)
                throw new ArgumentNullException(nameof(csv));

            var extension = Path.GetExtension(csv.FileName);

            if (!_allowedExtensions.Contains(extension.ToLower()))
                throw new ArgumentNullException(nameof(csv));
        }

        public async Task Create(Requests.Create request)
        {
            ValidateCsvDumpFile(request.CsvDumpFile);

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

        private IEnumerable<AccessPointFull> ParseAccessPointDumps(IFormFile csvFile)
        {
            using var sr = new StreamReader(csvFile.OpenReadStream());

            using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);

            var accessPoints = new Dictionary<string, AccessPointFull>();

            foreach (var record in csv.GetRecords<AccessPointRecord>())
            {
                if (record.Type != "AP") continue;

                if (!accessPoints.ContainsKey(record.Bssid))
                {
                    accessPoints.Add(record.Bssid, record.ToFull());
                    continue;
                }

                var accessPoint = accessPoints[record.Bssid];

                if (record.Power < accessPoint.LowPower)
                {
                    accessPoint.LowPower = record.Power;
                    accessPoint.LowLatitude = record.Latitude;
                    accessPoint.LowLongitude = record.Longitude;
                }

                if (record.Power > accessPoint.HighPower)
                {
                    accessPoint.HighPower = record.Power;
                    accessPoint.HighLatitude = record.Latitude;
                    accessPoint.HighLongitude = record.Longitude;
                }

                accessPoints[record.Bssid] = accessPoint;
            }

            return accessPoints.Select(a => a.Value);
        }

        private async Task CreateAccessPoint(AccessPointFull record)
        {
            var accessPoint = AccessPoint.Factory.Create(new Events.V1.AccessPointCreated
            {
                Bssid = record.Bssid,
                Ssid = record.Ssid,
                Frequency = _defaultFrequencyValue,
                LowSignalLevel = record.LowPower,
                LowSignalLatitude = record.LowLatitude,
                LowSignalLongitude = record.LowLongitude,
                HighSignalLevel = record.HighPower,
                HighSignalLatitude = record.HighLatitude,
                HighSignalLongitude = record.HighLongitude,
                RawSecurityPayload = record.Security,
                UserId = _scopeWrapperService.GetUserId()
            });

            await _unitOfWork.AccessPointRepository.Add(accessPoint);
        }

        private async Task CreateAccessPointStamp(AccessPointFull record)
        {
            var accessPoint = await _unitOfWork.AccessPointRepository.Get(record.Bssid);

            accessPoint.Apply(new Events.V1.AccessPointStampCreated
            {
                Id = accessPoint.Id,
                Ssid = record.Ssid,
                Frequency = _defaultFrequencyValue,
                LowSignalLevel = record.LowPower,
                LowSignalLatitude = record.LowLatitude,
                LowSignalLongitude = record.LowLongitude,
                HighSignalLevel = record.HighPower,
                HighSignalLatitude = record.HighLatitude,
                HighSignalLongitude = record.HighLongitude,
                RawSecurityPayload = record.Security,
                UserId = _scopeWrapperService.GetUserId()
            });
        }
    }
}
