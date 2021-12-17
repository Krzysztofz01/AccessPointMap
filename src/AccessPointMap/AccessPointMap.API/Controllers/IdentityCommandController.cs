using AccessPointMap.API.Controllers.Base;
using AccessPointMap.Application.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public IdentityCommandController(IIdentityService identityService) : base()
        {
            _identityService = identityService ??
                throw new ArgumentNullException(nameof(identityService));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(V1.Delete command) =>
            await Command(command, _identityService.Handle);

        [HttpPut("activation")]
        public async Task<IActionResult> Activation(V1.Activation command) =>
            await Command(command, _identityService.Handle);

        [HttpPut("role")]
        public async Task<IActionResult> RoleChange(V1.RoleChange command) =>
            await Command(command, _identityService.Handle);
    }
}
