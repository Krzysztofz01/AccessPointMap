using AccessPointMap.Domain;
using AccessPointMap.Repository;
using AccessPointMap.Service.Integration.Wigle.Models;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace AccessPointMap.Service.Integration.Wigle
{
    public class WigleIntegration : IntegrationBase, IWigleIntegration
    {
        private static readonly string _integrationName = "Wiggle Integration";
        private readonly int _defaultFrequency = 0;

        private readonly IAccessPointHelperService _accessPointHelperService;

        public WigleIntegration(IAccessPointRepository accessPointRepository, IAccessPointHelperService accessPointHelperService) : base(accessPointRepository, _integrationName)
        {
            _accessPointHelperService = accessPointHelperService ??
                throw new ArgumentNullException(nameof(accessPointHelperService));
        }

        public async Task Add(IFormFile file, long userId)
        {
            FileValidation(file);

            using var reader = new StreamReader(file.OpenReadStream());

            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var accessPoints = new List<AccessPoint>();
            foreach (var record in csv.GetRecords<AccessPointRecord>())
            {
                if (!RecordValidation(record)) continue;

                var accessPoint = AccessPoint.Factory.Create(
                    record.Mac,
                    record.Ssid,
                    _defaultFrequency,
                    record.Rssi,
                    record.Latitude,
                    record.Longitude,
                    record.Rssi,
                    record.Latitude,
                    record.Longitude,
                    record.AuthMode,
                    userId);

                accessPoint.SetSerializedSecurityData(_accessPointHelperService.SerializeSecurityData(record.AuthMode));
                
                accessPoint.SetSecurityStatus(_accessPointHelperService.CheckIsSecure(record.AuthMode));

                accessPoint.SetDeviceType(_accessPointHelperService.DetectDeviceType(record.Ssid));

                accessPoints.Add(accessPoint);
            }

            await AddToQueue(accessPoints);

            await Commit();
        }

        private void FileValidation(IFormFile file)
        {
            if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException(nameof(file), "Invalid access point data file format.");
        }

        private bool RecordValidation(AccessPointRecord record)
        {
            if (record.Type != "WIFI") return false;

            return true;
        }
    }
}
