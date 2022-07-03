using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Domain.AccessPoints.AccessPointPackets
{
    public class AccessPointPacketFrameType : ValueObject<AccessPointPacketFrameType>
    {
        public string Value { get; private set; }

        private AccessPointPacketFrameType() { }
        private AccessPointPacketFrameType(string value)
        {
            if (value.IsEmpty())
                throw new ValueObjectValidationException("Invalid IEEE 802.11 frame type name.");
            
            Value = value;
        }

        public static implicit operator string(AccessPointPacketFrameType frameType) => frameType.Value;

        public static AccessPointPacketFrameType FromString(string frameType) => new AccessPointPacketFrameType(frameType);
    }
}
