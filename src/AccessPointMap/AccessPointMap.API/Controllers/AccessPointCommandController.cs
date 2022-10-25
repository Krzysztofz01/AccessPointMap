using AccessPointMap.API.Controllers.Base;
using AccessPointMap.Application.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using static AccessPointMap.Application.AccessPoints.Commands;

namespace AccessPointMap.API.Controllers
{
    [Route("api/v{version:apiVersion}/accesspoints")]
    [ApiVersion("1.0")]
    public class AccessPointCommandController : CommandController
    {
        private readonly IAccessPointService _accessPointService;

        public AccessPointCommandController(IAccessPointService accessPointService, ILogger<AccessPointCommandController> logger) : base(logger)
        {
            _accessPointService = accessPointService ??
                throw new ArgumentNullException(nameof(accessPointService));
        }

        [HttpPost]
        public async Task<IActionResult> Create(V1.Create command, CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(command, _accessPointService, cancellationToken);

        [HttpDelete]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> Delete(V1.Delete command, CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(command, _accessPointService, cancellationToken);

        [HttpDelete("range")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> DeleteRange(V1.DeleteRange command, CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(command, _accessPointService, cancellationToken);

        [HttpPut]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> Update(V1.Update command, CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(command, _accessPointService, cancellationToken);

        [HttpPut("display")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> DisplayStatusChange(V1.ChangeDisplayStatus command, CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(command, _accessPointService, cancellationToken);

        [HttpPut("range/display")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> DisplayStatusChangeRange(V1.ChangeDisplayStatusRange command, CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(command, _accessPointService, cancellationToken);

        [HttpPut("merge")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> MergeWithStamp(V1.MergeWithStamp command, CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(command, _accessPointService, cancellationToken);

        [HttpDelete("stamp")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> DeleteStamp(V1.DeleteStamp command, CancellationToken cancellationToken) =>
            await ExecuteCommandAsync(command, _accessPointService, cancellationToken);
    }
}
