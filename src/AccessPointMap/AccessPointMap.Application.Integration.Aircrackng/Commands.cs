using AccessPointMap.Application.Integration.Core;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AccessPointMap.Application.Integration.Aircrackng
{
    public static class Commands
    {
        public class CreateAccessPointsFromCsvFile : IIntegrationCommand
        {
            [Required]
            public IFormFile ScanCsvFile { get; set; }
        }

        public class CreatePacketsFromPcapFile : IIntegrationCommand
        {
            [Required]
            public IFormFile ScanPcapFile { get; set; }
        }
    }
}
