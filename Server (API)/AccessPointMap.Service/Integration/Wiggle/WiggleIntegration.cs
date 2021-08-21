using AccessPointMap.Domain;
using AccessPointMap.Repository;
using AccessPointMap.Service.Integration.Wiggle.Models;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace AccessPointMap.Service.Integration.Wiggle
{
    public class WiggleIntegration : IntegrationBase, IWiggleIntegration
    {
        private static readonly string _integrationName = "Wiggle Integration";

        private readonly int _defaultFrequency = 0;

        public WiggleIntegration(IAccessPointRepository accessPointRepository) : base(accessPointRepository, _integrationName)
        {
        }

        public async Task Add(IFormFile file, long userId)
        {
            FileValidation(file);

            using var reader = new StreamReader(file.OpenReadStream());

            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var accessPoints = new List<AccessPoint>();
            foreach (var record in csv.GetRecords<AccessPointRecord>())
            {
                var accessPoint = AccessPoint.Factory.Create(
                    record.Bssid,
                    record.Ssid,
                    _defaultFrequency,
                    record.MaxSignalLevel,
                    record.MaxSignalLatitude,
                    record.MaxSignalLongitude,
                    record.MaxSignalLevel,
                    record.MaxSignalLatitude,
                    record.MaxSignalLongitude,
                    record.FullSecurityData,
                    userId);

                // Set serialized data

                // Set is secure

                accessPoint.SetDeviceType(record.DeviceType);

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
    }
}
