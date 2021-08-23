using AccessPointMap.Service;
using AccessPointMap.Service.Integration.Aircrackng;
using AccessPointMap.Service.Integration.Wigle;
using AccessPointMap.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AccessPointMap.Web.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/integration")]
    [ApiVersion("1.0")]
    public class IntegrationController : ControllerBase
    {
        private readonly IWigleIntegration _wigleIntegration;
        private readonly IAircrackngIntegration _aircrackngIntegration;
        private readonly IUserService _userService;
        private readonly ILogger<IntegrationController> _logger;

        public IntegrationController(
            IWigleIntegration wigleIntegration,
            IAircrackngIntegration aircrackngIntegration,
            IUserService userService,
            ILogger<IntegrationController> logger)
        {
            _wigleIntegration = wigleIntegration ??
                throw new ArgumentNullException(nameof(wigleIntegration));

            _aircrackngIntegration = aircrackngIntegration ??
                throw new ArgumentNullException(nameof(aircrackngIntegration));

            _userService = userService ??
                throw new ArgumentNullException(nameof(userService));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("wigle")]
        [Authorize]
        public async Task<IActionResult> UploadWigleDataV1([FromForm] AccessPointIntegrationPostView form)
        {
            try
            {
                var userId = _userService.GetUserIdFromPayload(User.Claims);

                await _wigleIntegration.Add(form.File, userId);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"System failure on adding accesspoints via Wigle integration.");
                return Problem();
            }
        }

        [HttpPost("aircrackng")]
        [Authorize]
        public async Task<IActionResult> UploadAircrackngDataV1([FromForm] AccessPointIntegrationPostView form)
        {
            try
            {
                var userId = _userService.GetUserIdFromPayload(User.Claims);

                await _aircrackngIntegration.Add(form.File, userId);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"System failure on adding accesspoints via Aircrack-ng integration.");
                return Problem();
            }
        }
    }
}
