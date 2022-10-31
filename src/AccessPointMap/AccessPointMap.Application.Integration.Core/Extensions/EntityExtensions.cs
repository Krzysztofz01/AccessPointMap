using AccessPointMap.Application.Logging;
using AccessPointMap.Domain.Core.Events;
using AccessPointMap.Domain.Core.Models;
using Microsoft.Extensions.Logging;

namespace AccessPointMap.Application.Integration.Core.Extensions
{
    public static class EntityExtensions
    {
        public static void ApplyWithLogging<TEntity, TCategoryName>(this TEntity entity, IEvent @event, ILogger<TCategoryName> logger) where TEntity : Entity
        {
            logger.LogDomainEvent(@event);

            entity.Apply(@event);
        }
    }
}
