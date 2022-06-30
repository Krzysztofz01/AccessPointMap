using System.Linq;

namespace AccessPointMap.Application.Pcap.ApmPcapNative.Extensions
{
    internal static class StringExtensions
    {
        public static string SkipLast(this string value)
        {
            return string.Concat(value.SkipLast(1));
        }
    }
}
