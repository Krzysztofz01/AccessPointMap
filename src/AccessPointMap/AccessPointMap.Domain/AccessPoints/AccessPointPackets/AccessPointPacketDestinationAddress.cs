using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Domain.AccessPoints.AccessPointPackets
{
    public class AccessPointPacketDestinationAddress : ValueObject<AccessPointPacketDestinationAddress>
    {
        //private const int _addressLength = 17;

        public string Value { get; private set; }

        private AccessPointPacketDestinationAddress() { }
        private AccessPointPacketDestinationAddress(string value)
        {
            //TODO: Some packets doesnt contain addresses, that means this value object can be string.empty,
            //      the validation should be moved to the ,,level'' on the create event step. The validation
            //      code below will be removed.

            //if (value.IsEmpty())
            //    throw new ValueObjectValidationException("The access point packet destination address can not be empty.");

            //if (value.Length != _addressLength)
            //    throw new ValueObjectValidationException("Invalud access point packet destination address format");

            Value = value.ToUpper();
        }

        public static implicit operator string(AccessPointPacketDestinationAddress address) => address.Value;

        public static AccessPointPacketDestinationAddress FromString(string address) => new AccessPointPacketDestinationAddress(address);
    }
}
