using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointBssid : ValueObject<AccessPointBssid>
    {
        private const int _bssidLength = 17;

        public string Value { get; private set; }

        private AccessPointBssid() { }
        private AccessPointBssid(string value)
        {
            if (value.IsEmpty())
                throw new ValueObjectValidationExceptions("The access point bssid can not be empty.");

            if (value.Length != _bssidLength)
                throw new ValueObjectValidationExceptions("Invalud access point bssid format");

            Value = value;
        }

        public static implicit operator string(AccessPointBssid bssid) => bssid.Value;

        public static AccessPointBssid FromString(string bssid) => new AccessPointBssid(bssid);
    }
}
