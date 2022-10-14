using AccessPointMap.Application.Pcap.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Pcap.ApmPcapNative
{
    internal sealed class ApmPcapNativePcapParsingService : IPcapParsingService
    {
        private readonly string[] _allowedExtensions = new string[] { ".cap", ".pcap", "pcapng" };

        public async Task<IDictionary<string, List<Packet>>> MapPacketsToMacAddressesAsync(IFormFile pcapFile, CancellationToken cancellationToken = default)
        {
            ValidatePcapFile(pcapFile);

            using var fileMemoryStream = new MemoryStream();
            await pcapFile.CopyToAsync(fileMemoryStream, cancellationToken);
            fileMemoryStream.Seek(0, SeekOrigin.Begin); 

            using var pcapParser = new PcapParser(fileMemoryStream);
            var parsedPackets = pcapParser.ParsePackets(cancellationToken);

            return parsedPackets
                .GroupBy(k => k.SourceAddress)
                .Where(k => k.Key != string.Empty)
                .ToDictionary(k => k.Key, v => v.ToList());
        }

        private void ValidatePcapFile(IFormFile pcapFile)
        {
            if (pcapFile is null)
                throw new ArgumentNullException(nameof(pcapFile));

            var extension = Path.GetExtension(pcapFile.FileName);

            if (!_allowedExtensions.Contains(extension.ToLower()))
                throw new ArgumentNullException(nameof(pcapFile));
        }
    }
}
