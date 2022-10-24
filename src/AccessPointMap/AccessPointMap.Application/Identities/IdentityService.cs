using AccessPointMap.Application.Abstraction;
using AccessPointMap.Application.Logging;
using AccessPointMap.Domain.Core.Events;
using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Identities;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
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

        public async Task<Result> HandleAsync(IApplicationCommand<Identity> command, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogApplicationCommand(command);

                switch (command)
                {
                    case V1.Delete c: await ApplyAsync(c.Id, new IdentityDeleted { Id = c.Id }, cancellationToken); break;
                    case V1.Activation c: await ApplyAsync(c.Id, new IdentityActivationChanged { Id = c.Id, Activated = c.Activated.Value }, cancellationToken); break;
                    case V1.RoleChange c: await ApplyAsync(c.Id, new IdentityRoleChanged { Id = c.Id, Role = c.Role.Value }, cancellationToken); break;

                    default: return Result.Failure(Error.FromString("This command is not supported."));
                }

                return Result.Success("Identity command applied successful.");
            }
            catch (DomainException ex)
            {
                var error = Error.FromException(ex);

                return Result.Failure(error);
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch
            {
                throw;
            }
        }

        private async Task ApplyAsync(Guid id, IEventBase @event, CancellationToken cancellationToken = default)
        {
            var identity = await _unitOfWork.IdentityRepository.GetAsync(id, cancellationToken);

            _logger.LogDomainEvent(@event);

            identity.Apply(@event);

            await _unitOfWork.Commit(cancellationToken);
        }
    }
}
