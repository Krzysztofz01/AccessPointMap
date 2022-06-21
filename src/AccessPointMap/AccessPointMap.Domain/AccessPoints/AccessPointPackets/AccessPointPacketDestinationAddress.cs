using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Domain.AccessPoints.AccessPointPackets
{
    public class AccessPointPacketDestinationAddress : ValueObject<AccessPointPacketDestinationAddress>
    {
        private const int _addressLength = 17;

        public string Value { get; private set; }

        private AccessPointPacketDestinationAddress() { }
        private AccessPointPacketDestinationAddress(string value)
        {
            if (value.IsEmpty())
                throw new ValueObjectValidationException("The access point packet destination address can not be empty.");

            if (value.Length != _addressLength)
                throw new ValueObjectValidationException("Invalud access point packet destination address format");

            Value = value.ToUpper();
        }

        public static implicit operator string(AccessPointPacketDestinationAddress address) => address.Value;

        public static AccessPointPacketDestinationAddress FromString(string address) => new AccessPointPacketDestinationAddress(address);
    }
}
