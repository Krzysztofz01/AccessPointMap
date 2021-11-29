using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointDeviceType : ValueObject<AccessPointDeviceType>
    {
        private readonly IReadOnlyDictionary<IEnumerable<string>, string> _deviceTypeDictionary = new Dictionary<IEnumerable<string>, string>
        {
            { new string[] { "printer", "print", "jet" }, "Printer" },
            { new string[] { "hotspot" }, "Access point" },
            { new string[] { "tv", "bravia" }, "Tv" },
            { new string[] { "cctv", "cam", "iptv", "monitoring" }, "Cctv" },
            { new string[] { "repeater", "extender" }, "Repeater" },
            { new string[] { "iot" }, "IoT" }
        };

        private const string _defaultDeviceType = "Unknown";

        public string Value { get; private set; }

        private AccessPointDeviceType() { }
        private AccessPointDeviceType(string value)
        {
            if (value.IsEmpty())
            {
                Value = _defaultDeviceType;
                return;
            }

            Value = _deviceTypeDictionary
                .Where(d => d.Key
                    .Any(k => k.Contains(value
                        .Trim()
                        .ToLower())))
                    .Select(k => k.Value)
                .FirstOrDefault();

            if (Value is null)
                Value = _defaultDeviceType;
        }

        public static implicit operator string(AccessPointDeviceType deviceType) => deviceType.Value;

        public static AccessPointDeviceType FromString(string ssid) => new AccessPointDeviceType(ssid);
        public static AccessPointDeviceType Unknown => new AccessPointDeviceType(null);
    }
}
