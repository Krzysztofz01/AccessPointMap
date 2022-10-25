using AccessPointMap.API.Controllers.Base;
using AccessPointMap.Application.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using static AccessPointMap.Application.Identities.Commands;

namespace AccessPointMap.API.Controllers
{
    [Route("api/v{version:apiVersion}/identities")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class IdentityCommandController : CommandController
    {
        private readonly IIdentityService _identityService;

        public IdentityCommandController(IIdentityService identityService, ILogger<IdentityCommandController> logger) : base(logger)
        {
            _identityService = identityService ??
                throw new ArgumentNullException(nameof(identityService));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(V1.Delete command, CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(command, _identityService, cancellationToken);

        [HttpPut("activation")]
        public async Task<IActionResult> Activation(V1.Activation command, CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(command, _identityService, cancellationToken);

        [HttpPut("role")]
        public async Task<IActionResult> RoleChange(V1.RoleChange command, CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(command, _identityService, cancellationToken);
    }
}
