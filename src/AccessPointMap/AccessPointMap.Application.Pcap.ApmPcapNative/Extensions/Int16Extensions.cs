using System;
using System.Linq;

namespace AccessPointMap.Application.Pcap.ApmPcapNative.Extensions
{
    internal static class Int16Extensions
    {
        public static bool GetBit(this ushort value, int bitIndex)
        {
            if (bitIndex < 0 || bitIndex > 15)
                throw new ArgumentOutOfRangeException(nameof(bitIndex), "Invalid bit index");

            var bitArr = Enumerable.Range(0, 16)
                .Select(bitIndex => 1 << bitIndex)
                .Select(bitMask => (value & bitMask) == bitMask)
                .Reverse()
                .ToArray();

            return bitArr[bitIndex];
        }
    }
}
