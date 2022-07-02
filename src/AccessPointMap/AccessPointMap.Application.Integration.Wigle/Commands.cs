using AccessPointMap.Application.Integration.Core;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AccessPointMap.Application.Integration.Wigle
{
    public static class Commands
    {
        public class CreateAccessPointsFromCsvFile : IIntegrationCommand
        {
            [Required]
            public IFormFile ScanCsvFile { get; set; }
        }
    }
}
