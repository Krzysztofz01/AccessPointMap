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

        public static readonly IReadOnlyCollection<SecurityStandard> SecurityStandards = new List<SecurityStandard>
        {
            new SecurityStandard { Name = "WEP", FullName = "Wired Equivalent Privacy", IsSecure = false },
            new SecurityStandard { Name = "WEP2", FullName = "Wired Equivalent Privacy 2", IsSecure = false },
            new SecurityStandard { Name = "WEP+", FullName = "Wired Equivalent Privacy Plus", IsSecure = false },
            new SecurityStandard { Name = "Dynamic WEP", FullName = "Dynamic Wired Equivalent Privacy", IsSecure = false },
            new SecurityStandard { Name = "WPS", FullName = "Wi-Fi Simple Config", IsSecure = false },
            new SecurityStandard { Name = "WPA", FullName = "Wi-Fi Protected Access", IsSecure = false },
            new SecurityStandard { Name = "WPA2", FullName = "Wi-Fi Protected Access II", IsSecure = true },
            new SecurityStandard { Name = "WPA3", FullName = "Wi-Fi Protected Access 3", IsSecure = true }
        };

        public static readonly IReadOnlyCollection<SecurityProtocols> SecurityProtocols = new List<SecurityProtocols>
        {
            new SecurityProtocols { Name = "CRC-32", FullName = "Cyclic redundancy check", IsSecure = false, Usage = "Integraity" },
            new SecurityProtocols { Name = "RC4", FullName = "Rivest Cipher 4", IsSecure = false, Usage = "Encryption" },
            new SecurityProtocols { Name = "TKIP", FullName = "Temporal Key Integrity Protocol", IsSecure = false, Usage = "Integrity" },
            new SecurityProtocols { Name = "PSK", FullName = "Pre-shared key", IsSecure = false, Usage = "Key exchange" },
            new SecurityProtocols { Name = "CCMP", FullName = "Counter Mode CBC-MAC Protocol", IsSecure = true, Usage = "Encryption" },
            new SecurityProtocols { Name = "AES", FullName = "Advanced Encryption Standard", IsSecure = true, Usage = "Encryption" },
            new SecurityProtocols { Name = "SHA", FullName = "Secure Hash Algorithm", IsSecure = true, Usage = "Integraity" },
            new SecurityProtocols { Name = "GCM", FullName = "Galois/Counter Mode", IsSecure = true, Usage = "Encryption Mode" },
            new SecurityProtocols { Name = "CCM", FullName = "Counter with CBC-MAC", IsSecure = true, Usage = "Encryption Mode" },
            new SecurityProtocols { Name = "SAE", FullName = "Simultaneous Authentication of Equals", IsSecure = true, Usage = "Key exchange" },
            new SecurityProtocols { Name = "PBKDF", FullName = "Password-Based Key Derivation Function", IsSecure = true, Usage = "Key derivation" },
            new SecurityProtocols { Name = "RADIUS", FullName = "Remote Authentication Dial-In User Service", IsSecure = true, Usage = "Authentication" },
            new SecurityProtocols { Name = "EAP", FullName = "Counter with CBC-MAC", IsSecure = true, Usage = "Authentication" },
            new SecurityProtocols { Name = "CCM", FullName = "Counter with CBC-MAC", IsSecure = true, Usage = "Encryption Mode" },
        };
    }

    public class SecurityStandard
    {
        public string FullName { get; init; }
        public string Name { get; init; }
        public bool IsSecure { get; init; }
    }

    public class SecurityProtocols
    {
        public string FullName { get; init; }
        public string Name { get; init; }
        public bool IsSecure { get; init; }
        public string Usage { get; init; }
    }

    public class EncryptionType
    {
        public string Name { get; init; }
        public bool IsSecure { get; init; }
        public int Priority { get; init; }
    }
}
