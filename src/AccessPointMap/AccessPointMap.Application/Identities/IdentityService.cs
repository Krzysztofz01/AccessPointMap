using AccessPointMap.Application.Core;
using AccessPointMap.Application.Core.Abstraction;
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
        private readonly IScopeWrapperService _scopeWrapperService;
        private readonly ILogger<IdentityService> _logger;

        public IdentityService(
            IUnitOfWork unitOfWork,
            IScopeWrapperService scopeWrapperService,
            ILogger<IdentityService> logger)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _scopeWrapperService = scopeWrapperService ??
                throw new ArgumentNullException(nameof(scopeWrapperService));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result> HandleAsync(IApplicationCommand<Identity> command, CancellationToken cancellationToken = default)
        {
            try
            {
                LogCommand(command);

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
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // TODO: Currently the domain layer code is using exceptions that are not derived from the DomainException base class.
                // In order to 500 Internal Server Error response codes on domain logic errors all exceptions are catched here.

                _logger.LogSuppressedException(ex);

                return Result.Failure(Error.FromException(ex));
            }
        }

        private async Task ApplyAsync(Guid id, IEventBase @event, CancellationToken cancellationToken = default)
        {
            var identity = await _unitOfWork.IdentityRepository.GetAsync(id, cancellationToken);

            _logger.LogDomainEvent(@event, id.ToString());

            identity.Apply(@event);

            await _unitOfWork.Commit(cancellationToken);
        }

        private void LogCommand(IApplicationCommand<Identity> command)
        {
            var userId = _scopeWrapperService.GetUserIdOrDefault();
            var userIdString = userId.HasValue ? userId.Value.ToString() : string.Empty;

            _logger.LogApplicationCommand(command, userIdString);
        }
    }
}
