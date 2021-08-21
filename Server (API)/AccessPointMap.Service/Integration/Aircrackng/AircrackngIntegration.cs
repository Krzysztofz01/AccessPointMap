using AccessPointMap.Repository;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace AccessPointMap.Service.Integration.Aircrackng
{
    public class AircrackngIntegration : IntegrationBase, IAircrackngIntegration
    {
        private readonly static string _integrationName = "Aircrack-ng";

        public AircrackngIntegration(IAccessPointRepository accessPointRepository) : base(accessPointRepository, _integrationName)
        {
        }

        public async Task Add(IFormFile file, long userId)
        {
            FileValidation(file);

            using var reader = new StreamReader(file.OpenReadStream());

            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            //container and loop
            throw new NotImplementedException();
        }

        private void FileValidation(IFormFile file)
        {
            if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException(nameof(file), "Invalid access point data file format.");
        }
    }
}
