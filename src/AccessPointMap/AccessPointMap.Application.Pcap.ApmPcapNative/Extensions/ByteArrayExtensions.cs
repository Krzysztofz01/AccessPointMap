using System;
using System.IO;
using System.Linq;

namespace AccessPointMap.Application.Pcap.ApmPcapNative.Extensions
{
    internal static class ByteArrayExtensions
    {
        public static ushort ToUInt16(this byte[] byteArray, int offset, bool reverseEndianness = false)
        {
            var uint16Buffer = byteArray.Skip(offset).Take(2).ToArray();
            
            return BitConverter.ToUInt16(reverseEndianness ? uint16Buffer.Reverse().ToArray() : uint16Buffer, 0);
        }

        public static byte[] ToHardwareAddressBuffer(this byte[] byteArray, int offset = 0)
        {
            return byteArray.Skip(offset).Take(6).ToArray();
        }
    }
}
