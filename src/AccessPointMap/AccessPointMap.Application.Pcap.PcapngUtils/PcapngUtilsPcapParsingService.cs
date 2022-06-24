using AccessPointMap.Application.Pcap.Core;
using Haukcode.PcapngUtils.Common;
using Haukcode.PcapngUtils.Pcap;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Pcap.PcapngUtils
{
    public class PcapngUtilsPcapParsingService : IPcapParsingService
    {
        private readonly string[] _allowedExtensions = new string[] { ".pcap", ".pcapng" };

        public async Task<IDictionary<string, IList<Packet>>> MapPacketsToMacAddresses(IFormFile pcapFile)
        {
            ValidatePcapFile(pcapFile);

            using var pcapFileStream = new MemoryStream();
            await pcapFile.CopyToAsync(pcapFileStream);
            pcapFileStream.Seek(0, SeekOrigin.Begin);

            using var pcapReader = new PcapReader(pcapFileStream);

            var packetMap = new Dictionary<string, IList<Packet>>();

            pcapReader.OnReadPacketEvent += (object context, IPacket packet) =>
            {
                var packetData = packet.Data;

                //TODO: Implementation of the data parsing
            };

            return packetMap;
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
