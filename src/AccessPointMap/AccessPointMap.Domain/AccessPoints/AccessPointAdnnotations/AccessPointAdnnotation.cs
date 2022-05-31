using AccessPointMap.Domain.Core.Events;
using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Models;
using System;
using static AccessPointMap.Domain.AccessPoints.Events;

namespace AccessPointMap.Domain.AccessPoints.AccessPointAdnnotations
{
    public class AccessPointAdnnotation : Entity
    {
        public AccessPointAdnnotationTitle Title { get; private set; }
        public AccessPointAdnnotationContent Content { get; private set; }
        public AccessPointAdnnotationTimestamp Timestamp { get; private set; }

        protected override void Handle(IEventBase @event)
        {
            switch (@event)
            {
                case V1.AccessPointAdnnotationDeleted e: When(e); break;

                default: throw new BusinessLogicException("This entity can not handlethis type of event.");
            }    
        }

        protected override void Validate()
        {
            bool isNull = Title is null || Content is null | Timestamp is null;

            if (isNull)
                throw new BusinessLogicException("The accesspoint adnnotation entity properties can not be null.");
        }

        private void When(V1.AccessPointAdnnotationDeleted _)
        {
            DeletedAt = DateTime.Now;
        }

        private AccessPointAdnnotation() { }

        public static class Factory
        {
            public static AccessPointAdnnotation Create(V1.AccessPointAdnnotationCreated @event)
            {
                return new AccessPointAdnnotation
                {
                    Title = AccessPointAdnnotationTitle.FromString(@event.Title),
                    Content = AccessPointAdnnotationContent.FromString(@event.Content),
                    Timestamp = AccessPointAdnnotationTimestamp.Now
                };
            }
        }
    }
}
