using AccessPointMap.Application.Abstraction;
using AccessPointMap.Application.Logging;
using AccessPointMap.Domain.Core.Events;
using AccessPointMap.Domain.Identities;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static AccessPointMap.Application.Identities.Commands;
using static AccessPointMap.Domain.Identities.Events.V1;

namespace AccessPointMap.Application.Identities
{
    public class IdentityService : IIdentityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IdentityService> _logger;

        public IdentityService(IUnitOfWork unitOfWork, ILogger<IdentityService> logger)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(IApplicationCommand<Identity> command)
        {
            _logger.LogApplicationCommand(command);

            switch(command)
            {
                case V1.Delete c: await Apply(c.Id, new IdentityDeleted { Id = c.Id }); break;
                case V1.Activation c: await Apply(c.Id, new IdentityActivationChanged { Id = c.Id, Activated = c.Activated.Value }); break;
                case V1.RoleChange c: await Apply(c.Id, new IdentityRoleChanged { Id = c.Id, Role = c.Role.Value }); break;
                
                default: throw new InvalidOperationException("This command is not supported.");
            }
        }

        private async Task Apply(Guid id, IEventBase @event)
        {
            // TODO: Pass the CancellationToken to the repository method
            var identity = await _unitOfWork.IdentityRepository.GetAsync(id);

            _logger.LogDomainEvent(@event);

            identity.Apply(@event);

            await _unitOfWork.Commit();
        }
    }
}
