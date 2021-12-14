using AccessPointMap.API.Utility;
using AccessPointMap.Application.Integration.Aircrackng;
using AccessPointMap.Application.Integration.Wigle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccessPointMap.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/integrations")]
    [ApiVersion("1.0")]
    [Authorize]
    public class IntegrationCommandController : ControllerBase
    {
        private readonly IWigleIntegrationService _wigleIntegrationService;
        private readonly IAircrackngIntegrationService _aircrackIntegrationService;

        public IntegrationCommandController(IWigleIntegrationService wigleIntegrationService)
        {
            _wigleIntegrationService = wigleIntegrationService ??
                throw new ArgumentNullException(nameof(wigleIntegrationService));
        }

        [HttpPost("wigle")]
        public async Task<IActionResult> CreateFromWigle(Application.Integration.Wigle.Requests.Create request) =>
            await RequestHandler.IntegrationServiceCommand(request, _wigleIntegrationService.Create);

        [HttpPost("aircrackng")]
        public async Task<IActionResult> CreateFromAircrackng(Application.Integration.Aircrackng.Requests.Create request) =>
            await RequestHandler.IntegrationServiceCommand(request, _aircrackIntegrationService.Create);
    }
}
