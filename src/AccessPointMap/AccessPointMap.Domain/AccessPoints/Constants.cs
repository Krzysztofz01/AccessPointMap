using System.Collections.Generic;

namespace AccessPointMap.Domain.AccessPoints
{
    public static class Constants
    {
        public static readonly IReadOnlyDictionary<IEnumerable<string>, string> DeviceTypeDictionary = new Dictionary<IEnumerable<string>, string>
        {
            { new string[] { "printer", "print", "jet" }, "Printer" },
            { new string[] { "hotspot", "free" }, "Access point" },
            { new string[] { "tv", "bravia" }, "Tv" },
            { new string[] { "cctv", "cam", "iptv", "monitoring" }, "Cctv" },
            { new string[] { "repeater", "extender", "ext" }, "Repeater" },
            { new string[] { "iot" }, "IoT" },
            { new string[] { "car" }, "Car" }
        };

        public static readonly IReadOnlyCollection<EncryptionType> EncryptionTypes = new List<EncryptionType>
        {
            new EncryptionType { Name = "WPA3", IsSecure = true, Priority = 10 },
            new EncryptionType { Name = "WPA2", IsSecure = true, Priority = 9 },
            new EncryptionType { Name = "WPA", IsSecure = true, Priority = 8 },
            new EncryptionType { Name = "WPS", IsSecure = false, Priority = 7 },
            new EncryptionType { Name = "WEP", IsSecure = false, Priority = 6 }
        };

        //TODO: The hex values are wrong, update with the values from ApmPcapNative
        public static readonly IReadOnlyDictionary<ushort, string> Ieee80211FrameSubTypeToName = new Dictionary<ushort, string>
        {
            { 0x0019, "Block Ack" },
            { 0x001d, "Acknowledgement" },
            { 0x000d, "Action" },
            { 0x0008, "Beacon Frame" },
            { 0x001c, "Clear-to-send" },
            { 0x0020, "Data" },
            { 0x0024, "Null function" },
            { 0x0005, "Probe response" },
            { 0x0028, "QoS Data" },
            { 0x001b, "Request-to-send" }
        };

        //TODO: The hex values are wrong, update with the values from ApmPcapNative
        public static IEnumerable<ushort> Ieee80211FrameSubTypesWithoutHardwareAddresses = new List<ushort>
        {
            0x001d,
            0x001c
        };
    }

    public class EncryptionType
    {
        public string Name { get; init; }
        public bool IsSecure { get; init; }
        public int Priority { get; init; }
    }
}
