using AccessPointMap.Application.Integration.Core;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AccessPointMap.Application.Integration.Wireshark
{
    public static class Commands
    {
        public class CreatePacketsFromPcapFile : IIntegrationCommand
        {
            [Required]
            public IFormFile ScanPcapFile { get; set; } 
        }
    }
}
