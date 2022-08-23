using AccessPointMap.API.Controllers.Base;
using AccessPointMap.Application.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
        public async Task<IActionResult> Create(V1.Create command) =>
            await Command(command, _accessPointService.Handle);

        [HttpDelete]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> Delete(V1.Delete command) =>
            await Command(command, _accessPointService.Handle);

        [HttpDelete("range")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> DeleteRange(V1.DeleteRange command) =>
            await Command(command, _accessPointService.Handle);

        [HttpPut]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> Update(V1.Update command) =>
            await Command(command, _accessPointService.Handle);

        [HttpPut("display")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> DisplayStatusChange(V1.ChangeDisplayStatus command) =>
            await Command(command, _accessPointService.Handle);

        [HttpPut("range/display")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> DisplayStatusChangeRange(V1.ChangeDisplayStatusRange command) =>
            await Command(command, _accessPointService.Handle);

        [HttpPut("merge")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> MergeWithStamp(V1.MergeWithStamp command) =>
            await Command(command, _accessPointService.Handle);

        [HttpDelete("stamp")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> DeleteStamp(V1.DeleteStamp command) =>
            await Command(command, _accessPointService.Handle);
    }
}
