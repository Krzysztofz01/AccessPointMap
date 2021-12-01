using System;

namespace AccessPointMap.Domain.Core.Events
{
    public interface IEvent : IEventBase
    {
        Guid Id { get; set; }
    }
}
