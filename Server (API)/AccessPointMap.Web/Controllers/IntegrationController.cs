using AccessPointMap.Service;
using AccessPointMap.Service.Integration.Aircrackng;
using AccessPointMap.Service.Integration.Wiggle;
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
        private readonly IWiggleIntegration _wiggleIntegration;
        private readonly IAircrackngIntegration _aircrackngIntegration;
        private readonly IUserService _userService;
        private readonly ILogger<IntegrationController> _logger;

        public IntegrationController(
            IWiggleIntegration wiggleIntegration,
            IAircrackngIntegration aircrackngIntegration,
            IUserService userService,
            ILogger<IntegrationController> logger)
        {
            _wiggleIntegration = wiggleIntegration ??
                throw new ArgumentNullException(nameof(wiggleIntegration));

            _aircrackngIntegration = aircrackngIntegration ??
                throw new ArgumentNullException(nameof(aircrackngIntegration));

            _userService = userService ??
                throw new ArgumentNullException(nameof(userService));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("wiggle")]
        [Authorize]
        public async Task<IActionResult> UploadWiggleDataV1(AccessPointIntegrationPostView form)
        {
            try
            {
                var userId = _userService.GetUserIdFromPayload(User.Claims);

                await _wiggleIntegration.Add(form.File, userId);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"System failure on adding accesspoints via Wiggle integration.");
                return Problem();
            }
        }

        [HttpPost("aircrackng")]
        [Authorize]
        public async Task<IActionResult> UploadAircrackngDataV1(AccessPointIntegrationPostView form)
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
