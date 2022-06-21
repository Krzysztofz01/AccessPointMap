using AccessPointMap.Domain.Core.Events;
using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Models;
using System;
using static AccessPointMap.Domain.AccessPoints.Events;

namespace AccessPointMap.Domain.AccessPoints.AccessPointPackets
{
    public class AccessPointPacket : Entity
    {
        public AccessPointPacketDestinationAddress DestinationAddress { get; private set; }
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
            bool isNull = DestinationAddress is null || Data is null;

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
                return new AccessPointPacket
                {
                    DestinationAddress = AccessPointPacketDestinationAddress.FromString(@event.DestinationAddress),
                    Data = AccessPointPacketData.FromString(@event.Data)
                };
            }
        }
    }
}
