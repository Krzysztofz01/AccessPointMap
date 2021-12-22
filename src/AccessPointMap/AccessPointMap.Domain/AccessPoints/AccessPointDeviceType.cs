using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;
using System.Linq;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointDeviceType : ValueObject<AccessPointDeviceType>
    {
        private const string _defaultDeviceType = "Unknown";
        public static string DefaultDeviceType => _defaultDeviceType;

        public string Value { get; private set; }

        private AccessPointDeviceType() { }
        private AccessPointDeviceType(string value)
        {
            if (value.IsEmpty())
            {
                Value = _defaultDeviceType;
                return;
            }

            Value = Constants.DeviceTypeDictionary
                .Where(d => d.Key
                    .Any(k => value
                        .Trim()
                        .ToLower()
                        .Contains(k)))
                .Select(d => d.Value)
                .FirstOrDefault();
            
            if (Value is null)
                Value = _defaultDeviceType;
        }

        public static implicit operator string(AccessPointDeviceType deviceType) => deviceType.Value;

        public static AccessPointDeviceType FromString(string ssid) => new AccessPointDeviceType(ssid);
        public static AccessPointDeviceType Unknown => new AccessPointDeviceType(null);
    }
}
