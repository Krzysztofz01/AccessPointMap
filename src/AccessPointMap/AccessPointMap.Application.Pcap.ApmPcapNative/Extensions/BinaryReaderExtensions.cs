using System.IO;

namespace AccessPointMap.Application.Pcap.ApmPcapNative.Extensions
{
    internal static class BinaryReaderExtensions
    {
        public static void SkipBytes(this BinaryReader binaryReader, int count)
        {
            _ = binaryReader.ReadBytes(count);
        }
    }
}
