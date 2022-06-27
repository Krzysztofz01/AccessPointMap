using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Domain.AccessPoints.AccessPointPackets
{
    public class AccessPointPacketDestinationAddress : ValueObject<AccessPointPacketDestinationAddress>
    {
        public string Value { get; private set; }

        private AccessPointPacketDestinationAddress() { }
        private AccessPointPacketDestinationAddress(string value)
        {
            if (value.IsEmpty()) value = string.Empty;

            Value = value.ToUpper();
        }

        public static implicit operator string(AccessPointPacketDestinationAddress address) => address.Value;

        public static AccessPointPacketDestinationAddress FromString(string address) => new AccessPointPacketDestinationAddress(address);
    }
}
