using AccessPointMap.Domain.Core.Events;
using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;
using System;
using System.Linq;
using static AccessPointMap.Domain.AccessPoints.Events;

namespace AccessPointMap.Domain.AccessPoints.AccessPointPackets
{
    public class AccessPointPacket : Entity
    {
        public AccessPointPacketDestinationAddress DestinationAddress { get; private set; }
        public AccessPointPacketSubtype Subtype { get; private set; }
        public AccessPointPacketData Data { get; private set; }

        protected override void Handle(IEventBase @event)
        {
            switch (@event)
            {
                case V1.AccessPointPacketDeleted e: When(e); break;

                default: throw new BusinessLogicException("This entity can not handle this type of event.");
            }
        }

        protected override void Validate()
        {
            bool isNull = DestinationAddress is null || Data is null || Subtype is null;

            if (isNull)
                throw new BusinessLogicException("The accesspoint packet entity properties can not be null.");
        }

        private void When(V1.AccessPointPacketDeleted _)
        {
            DeletedAt = DateTime.Now;
        }

        private AccessPointPacket() { }

        public static class Factory
        {
            public static AccessPointPacket Create(V1.AccessPointPacketCreated @event)
            {
                if (!Constants.Ieee80211FrameSubTypesWithoutHardwareAddresses.Any(s => s == @event.Subtype))
                {
                    if (@event.DestinationAddress.IsEmpty())
                        throw new BusinessLogicException("The access point packet destination address can not be empty for this frame type.");
                }

                return new AccessPointPacket
                {
                    DestinationAddress = AccessPointPacketDestinationAddress.FromString(@event.DestinationAddress),
                    Subtype = AccessPointPacketSubtype.FromUInt16(@event.Subtype),
                    Data = AccessPointPacketData.FromString(@event.Data)
                };
            }
        }
    }
}
