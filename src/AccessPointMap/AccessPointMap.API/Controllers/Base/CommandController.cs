using AccessPointMap.Application.Abstraction;
using AccessPointMap.Application.Integration.Core;
using AccessPointMap.Application.Logging;
using AccessPointMap.Domain.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AccessPointMap.API.Controllers.Base
{
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class CommandController : ControllerBase
    {
        private readonly ILogger<CommandController> _logger;

        public CommandController(ILogger<CommandController> logger)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        protected async Task<IActionResult> Command<TAggregateRoot>(IApplicationCommand<TAggregateRoot> command, Func<IApplicationCommand<TAggregateRoot>, Task> serviceHandler) where TAggregateRoot : AggregateRoot
        {
            _logger.LogCommandController(command);

            await serviceHandler(command);

            return new OkResult();
        }

        protected async Task<IActionResult> ExecuteService<TRequest>(TRequest request, Func<TRequest, Task> serviceHandler) where TRequest : class
        {
            if (request is IIntegrationCommand command)
            {
                _logger.LogCommandController(command);
            }
            else
            {
                _logger.LogCommandController(request);
            }

            await serviceHandler(request);

            return new OkResult();
        }
    }
}
