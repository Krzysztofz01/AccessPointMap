using AccessPointMap.Application.Pcap.ApmPcapNative;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;
using System.Threading.Tasks;

namespace AccessPointMap.Development
{
    public class Program
    {
        public static async Task Main()
        {
            const string pcapFilePath = @"D:\Pobrane\apm-pcap.cap";

            using var fs = File.OpenRead(pcapFilePath);

            IFormFile file = new FormFile(fs, 0, fs.Length, null, Path.GetFileName(pcapFilePath));

            var pcapService = new ApmPcapNativePcapParsingService();
            var result = await pcapService.MapPacketsToMacAddresses(file);
        }
    }
}