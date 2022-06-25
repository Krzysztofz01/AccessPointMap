using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Domain.AccessPoints.AccessPointPackets
{
    public class AccessPointPacketSubtype : ValueObject<AccessPointPacketSubtype>
    {
        public string Value { get; private set; }

        private AccessPointPacketSubtype() { }
        private AccessPointPacketSubtype(ushort value)
        {
            if (!Constants.Ieee80211FrameSubTypeToName.ContainsKey(value))
                throw new ValueObjectValidationException("Invalid IEEE802.11 frame subtype identifier.");

            Value = Constants.Ieee80211FrameSubTypeToName[value];
        }

        public static implicit operator string(AccessPointPacketSubtype subtype) => subtype.Value;

        public static AccessPointPacketSubtype FromUInt16(ushort subtype) => new AccessPointPacketSubtype(subtype);
    }
}
