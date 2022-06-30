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


        public static readonly IReadOnlyDictionary<string, SecurityProtocol> SecurityProtocols = new Dictionary<string, SecurityProtocol>
        {
            // Standards
            { "WEP",  new SecurityProtocol { Name = "WEP", FullName = "Wired Equivalent Privacy", IsSecure = false, Type = SecurityProtocolType.Framework, Priority = 10 } },
            { "WEP2", new SecurityProtocol { Name = "WEP2", FullName = "Wired Equivalent Privacy 2", IsSecure = false, Type = SecurityProtocolType.Framework, Priority = 20 } },
            { "WEP+", new SecurityProtocol { Name = "WEP+", FullName = "Wired Equivalent Privacy Plus", IsSecure = false, Type = SecurityProtocolType.Framework, Priority = 30 } },
            { "Dynamic WEP", new SecurityProtocol { Name = "Dynamic WEP", FullName = "Dynamic Wired Equivalent Privacy", IsSecure = false, Type = SecurityProtocolType.Framework, Priority = 40 } },
            { "WPS", new SecurityProtocol { Name = "WPS", FullName = "Wi-Fi Simple Config", IsSecure = false, Type = SecurityProtocolType.Framework, Priority = 50 } },
            { "WPA", new SecurityProtocol { Name = "WPA", FullName = "Wi-Fi Protected Access", IsSecure = false, Type = SecurityProtocolType.Framework, Priority = 60 } },
            { "WPA2", new SecurityProtocol { Name = "WPA2", FullName = "Wi-Fi Protected Access II", IsSecure = true, Type = SecurityProtocolType.Framework, Priority = 70 } },
            { "WPA3", new SecurityProtocol { Name = "WPA3", FullName = "Wi-Fi Protected Access 3", IsSecure = true, Type = SecurityProtocolType.Framework, Priority = 80 } },

            // Protocols
            { "CRC-32", new SecurityProtocol { Name = "CRC-32", FullName = "Cyclic redundancy check", IsSecure = false, Type = SecurityProtocolType.Integrity } },
            { "RC4", new SecurityProtocol { Name = "RC4", FullName = "Rivest Cipher 4", IsSecure = false, Type = SecurityProtocolType.Encryption } },
            { "TKIP", new SecurityProtocol { Name = "TKIP", FullName = "Temporal Key Integrity Protocol", IsSecure = false, Type = SecurityProtocolType.Integrity } },
            { "PSK", new SecurityProtocol { Name = "PSK", FullName = "Pre-shared key", IsSecure = false, Type = SecurityProtocolType.KeyExchange } },
            { "CCMP", new SecurityProtocol { Name = "CCMP", FullName = "Counter Mode CBC-MAC Protocol", IsSecure = true, Type = SecurityProtocolType.Encryption } },
            { "AES", new SecurityProtocol { Name = "AES", FullName = "Advanced Encryption Standard", IsSecure = true, Type = SecurityProtocolType.Encryption } },
            { "SHA", new SecurityProtocol { Name = "SHA", FullName = "Secure Hash Algorithm", IsSecure = true, Type = SecurityProtocolType.Integrity } },
            { "GCM", new SecurityProtocol { Name = "GCM", FullName = "Galois/Counter Mode", IsSecure = true, Type = SecurityProtocolType.EncryptionMode } },
            { "CCM", new SecurityProtocol { Name = "CCM", FullName = "Counter with CBC-MAC", IsSecure = true, Type = SecurityProtocolType.EncryptionMode } },
            { "SAE", new SecurityProtocol { Name = "SAE", FullName = "Simultaneous Authentication of Equals", IsSecure = true, Type = SecurityProtocolType.KeyExchange } },
            { "PBKDF", new SecurityProtocol { Name = "PBKDF", FullName = "Password-Based Key Derivation Function", IsSecure = true, Type = SecurityProtocolType.KeyDerivation } },
            { "RADIUS", new SecurityProtocol { Name = "RADIUS", FullName = "Remote Authentication Dial-In User Service", IsSecure = true, Type = SecurityProtocolType.Authentication } },
            { "EAP", new SecurityProtocol { Name = "EAP", FullName = "Extensible Authentication Protocol", IsSecure = true, Type = SecurityProtocolType.Authentication } },
        };
    }
    public class SecurityProtocol
    {
        public string FullName { get; init; }
        public string Name { get; init; }
        public bool IsSecure { get; init; }
        public int Priority { get; init; } = 0;
        public SecurityProtocolType Type { get; init; }
    }

    public enum SecurityProtocolType
    {
        Framework,
        Integrity,
        Encryption,
        EncryptionMode,
        KeyExchange,
        KeyDerivation,
        Authentication
    }
}
