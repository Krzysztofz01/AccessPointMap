using System;
using System.IO;
using System.Linq;

namespace AccessPointMap.Application.Pcap.ApmPcapNative.Extensions
{
    internal static class ByteArrayExtensions
    {
        public static BinaryReader ToBinaryReader(this byte[] byteArray)
        {
            using var memoryStream = new MemoryStream(byteArray);
            return new BinaryReader(memoryStream);
        }

        public static ushort ToUInt16(this byte[] byteArray, int offset = 0)
        {
            return BitConverter.ToUInt16(byteArray, offset);
        }

        public static byte[] ToHardwareAddressBuffer(this byte[] byteArray, int offset = 0)
        {
            return byteArray.Skip(offset).Take(6).ToArray();
        }
    }
}
