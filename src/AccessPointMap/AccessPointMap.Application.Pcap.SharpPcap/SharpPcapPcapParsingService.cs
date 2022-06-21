using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Pcap.SharpPcap
{
    public class SharpPcapPcapParsingService : Core.IPcapParsingService
    {
        //TODO: Remove after debug
        private readonly ILogger<SharpPcapPcapParsingService> _logger;

        public SharpPcapPcapParsingService(ILogger<SharpPcapPcapParsingService> logger)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IDictionary<string, IList<Core.Packet>>> MapPacketsToMacAddresses(IFormFile pcapFile)
        {
            try
            {
                var pcapTempFilePath = await StoreTempPcapFile(pcapFile);

                ICaptureDevice captureReaderDevice = new CaptureFileReaderDevice(pcapTempFilePath);
                captureReaderDevice.Open();

                var packetMap = new Dictionary<string, IList<Core.Packet>>();

                captureReaderDevice.OnPacketArrival += (object sender, PacketCapture e) =>
                {
                    var rawPacket = e.GetPacket();
                    if (rawPacket is null || rawPacket.LinkLayerType != LinkLayers.Ethernet) return;

                    var ethernetPacket = (EthernetPacket)Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
                    if (ethernetPacket is null) return;
 
                    var sourceMacAddress = ethernetPacket.SourceHardwareAddress.ToString();
                    if (sourceMacAddress is null) return;

                    var parsedPacket = new Core.Packet
                    {
                        SourceAddress = sourceMacAddress,
                        DestinationAddress = ethernetPacket.DestinationHardwareAddress.ToString(),
                        Data = Convert.ToBase64String(rawPacket.Data)
                    };

                    if (packetMap.ContainsKey(sourceMacAddress))
                    {
                        packetMap[sourceMacAddress].Add(parsedPacket);
                    }
                    else
                    {
                        packetMap.Add(sourceMacAddress, new List<Core.Packet>() { parsedPacket });
                    }
                };

                captureReaderDevice.Capture();
                captureReaderDevice.Close();

                RemoveTempPcapFile(pcapTempFilePath);

                return packetMap;
            }
            catch
            {
                //TODO: Possible null reference. Warning CS8603
                return null;
            }
        }

        //TODO: File access should be handled by external service
        private async Task<string> StoreTempPcapFile(IFormFile pcapFile)
        {
            var tempFile = Path.GetTempFileName();

            using var fileStream = new FileStream(tempFile, FileMode.Append);

            await pcapFile.CopyToAsync(fileStream);

            return tempFile;
        }

        //TODO: File access should be handled by external service
        private void RemoveTempPcapFile(string pcapFileName)
        {
            if (!File.Exists(pcapFileName)) return;

            File.Delete(pcapFileName);
        }
    }
}
