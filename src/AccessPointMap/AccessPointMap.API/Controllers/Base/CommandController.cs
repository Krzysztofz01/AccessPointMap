using AccessPointMap.Application.Authentication;
using AccessPointMap.Application.Core;
using AccessPointMap.Application.Core.Abstraction;
using AccessPointMap.Application.Extensions;
using AccessPointMap.Application.Integration.Core;
using AccessPointMap.Application.Logging;
using AccessPointMap.Domain.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.API.Controllers.Base
{
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class CommandController : ControllerBase
    {
        private readonly ILogger<CommandController> _logger;

        public CommandController(ILogger<CommandController> logger)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        protected async Task<IActionResult> ExecuteCommandAsync<TAggregateRoot>(IApplicationCommand<TAggregateRoot> command, IApplicationService<TAggregateRoot> handlerService, CancellationToken cancellationToken = default) where TAggregateRoot : AggregateRoot
        {
            LogIncomingCommandReuqest(command);

            var result = await handlerService.HandleAsync(command, cancellationToken);

            if (result.IsFailure) return GetFailureResponse(result.Error);
            
            return new OkResult();
        }

        protected async Task<IActionResult> ExecuteIntegrationCommandAsync(IIntegrationCommand command, IIntegrationContract handlerIntegrationService, CancellationToken cancellationToken = default)
        {
            LogIncomingCommandReuqest(command);

            var result = await handlerIntegrationService.HandleCommandAsync(command, cancellationToken);

            if (result.IsFailure) return GetFailureResponse(result.Error);

            return new OkResult();
        }

        protected void LogIncomingCommandReuqest(ICommand command)
        {
            string currentPath = Request.GetEncodedPathAndQuery() ?? string.Empty;
            string currentHost = Request.GetIpAddressString() ?? string.Empty;
            string currentIdentityId = Request.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                "Anonymous";

            _logger.LogCommandController(command, currentPath, currentIdentityId, currentHost);
        }

        protected void LogIncomingAuthenticationRequest(IAuthenticationRequest request)
        {
            string currentPath = Request.GetEncodedPathAndQuery() ?? string.Empty;
            string currentHost = Request.GetIpAddressString() ?? string.Empty;
            string currentIdentityId = Request.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                "Anonymous";

            _logger.LogAuthenticationController(request, currentPath, currentIdentityId, currentHost);
        }

        private static IActionResult GetFailureResponse(Error error)
        {
            // TODO: Pass error message
            return new BadRequestResult();
        }
    }
}
