using AccessPointMap.Application.Integration.Core;
using Microsoft.AspNetCore.Http;

namespace AccessPointMap.Application.Integration.Aircrackng
{
    public static class Commands
    {
        public class CreateAccessPointsFromCsvFile : IIntegrationCommand
        {
            public IFormFile ScanCsvFile { get; set; }
        }

        public class CreatePacketsFromPcapFile : IIntegrationCommand
        {
            public IFormFile ScanPcapFile { get; set; }
        }
    }
}
