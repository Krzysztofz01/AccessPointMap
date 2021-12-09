using AccessPointMap.API.Utility;
using AccessPointMap.Application.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static AccessPointMap.Application.AccessPoints.Commands;

namespace AccessPointMap.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/accesspoints")]
    [ApiVersion("1.0")]
    [Authorize]
    public class AccessPointCommandController : ControllerBase
    {
        private readonly IAccessPointService _accessPointService;

        public AccessPointCommandController(IAccessPointService accessPointService)
        {
            _accessPointService = accessPointService ??
                throw new ArgumentNullException(nameof(accessPointService));
        }

        [HttpPost]
        public async Task<IActionResult> Create(V1.Create command) =>
            await RequestHandler.Command(command, _accessPointService.Handle);

        [HttpDelete]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> Delete(V1.Delete command) =>
            await RequestHandler.Command(command, _accessPointService.Handle);

        [HttpPut]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> Update(V1.Update command) =>
            await RequestHandler.Command(command, _accessPointService.Handle);

        [HttpPut("display")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> DisplayStatusChange(V1.ChangeDisplayStatus command) =>
            await RequestHandler.Command(command, _accessPointService.Handle);

        [HttpPut("merge")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> MergeWithStamp(V1.MergeWithStamp command) =>
            await RequestHandler.Command(command, _accessPointService.Handle);

        [HttpDelete("stamp")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> DeleteStamp(V1.DeleteStamp command) =>
            await RequestHandler.Command(command, _accessPointService.Handle);
    }
}
