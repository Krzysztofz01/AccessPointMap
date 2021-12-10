using AccessPointMap.Application.Integration.Core;
using AccessPointMap.Application.Integration.Wigle.Models;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Infrastructure.Core.Abstraction;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Integration.Wigle
{
    public class WigleIntegrationService : AccessPointIntegrationBase<WigleIntegrationService>, IWigleIntegrationService
    {
        private readonly string[] _allowedExtensions = new string[] { ".csv" };

        private const string _integrationName = "WiGLE";
        private const string _integrationDescription = "Integration for the bigest wardriving platform and their scanning application";
        private const string _integrationVersion = "0.0.0-alpha";

        private const double _defaultFrequencyValue = default;

        protected override string IntegrationName => _integrationName;
        protected override string IntegrationDescription => _integrationDescription;
        protected override string IntegrationVersion => _integrationVersion;

        public WigleIntegrationService(IUnitOfWork unitOfWork, IScopeWrapperService scopeWrapperService) : base(unitOfWork, scopeWrapperService) { }

        private void ValidateCsvDatabaseFile(IFormFile csv)
        {
            if (csv is null)
                throw new ArgumentNullException(nameof(csv));

            var extension = Path.GetExtension(csv.FileName);

            if (!_allowedExtensions.Contains(extension.ToLower()))
                throw new ArgumentNullException(nameof(csv));
        }

        public async Task Create(Requests.Create request)
        {
            ValidateCsvDatabaseFile(request.CsvDatabaseFile);

            using var sr = new StreamReader(request.CsvDatabaseFile.OpenReadStream());

            using var csv = new CsvReader(sr, CultureInfo.InvariantCulture);

            foreach (var record in csv.GetRecords<AccessPointRecord>())
            {
                if (await _unitOfWork.AccessPointRepository.Exists(record.Mac))
                {
                    await CreateAccessPointStamp(record);
                    continue;
                }

                await CreateAccessPoint(record);
            }

            await _unitOfWork.Commit();
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
                UserId = _scopeWrapperService.GetUserId()
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
                UserId = _scopeWrapperService.GetUserId()
            });
        }
    }
}
