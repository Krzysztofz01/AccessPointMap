using AccessPointMap.Application.Integration.Core;
using Microsoft.AspNetCore.Http;

namespace AccessPointMap.Application.Integration.Wireshark
{
    public static class Commands
    {
        public class CreatePacketsFromPcapFile : IIntegrationCommand
        {
            public IFormFile ScanPcapFile { get; set; } 
        }
    }
}
