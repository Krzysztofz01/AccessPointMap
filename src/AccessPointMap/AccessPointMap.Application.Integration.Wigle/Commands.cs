using AccessPointMap.Application.Integration.Core;
using Microsoft.AspNetCore.Http;

namespace AccessPointMap.Application.Integration.Wigle
{
    public static class Commands
    {
        public class CreateAccessPointsFromCsvFile : IIntegrationCommand
        {
            public IFormFile ScanCsvFile { get; set; }
        }
    }
}
