using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointSsid : ValueObject<AccessPointSsid>
    {
        private const string _hiddenNetworkName = "Hidden network";

        public string Value { get; private set; }

        private AccessPointSsid() { }
        private AccessPointSsid(string value)
        {
            if (value is null)
                throw new ValueObjectValidationException("Invalid access point ssid value.");

            if (value == string.Empty)
            {
                Value = _hiddenNetworkName;
                return;
            }

            Value = value;
        }

        public static implicit operator string(AccessPointSsid ssid) => ssid.Value;

        public static AccessPointSsid FromString(string ssid) => new AccessPointSsid(ssid);
        public static AccessPointSsid Hidden => new AccessPointSsid(string.Empty);
    }
}
