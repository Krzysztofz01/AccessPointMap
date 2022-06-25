using AccessPointMap.Application.Pcap.ApmPcapNative;
using System.IO;

namespace AccessPointMap.Development
{
    public class Program
    {
        public static void Main()
        {
            const string pcapFilePath = @"D:\Pobrane\apm-pcap.cap";

            using var pcapParser = new PcapParser(File.OpenRead(pcapFilePath));
            pcapParser.ParsePackets();
        }
    }
}