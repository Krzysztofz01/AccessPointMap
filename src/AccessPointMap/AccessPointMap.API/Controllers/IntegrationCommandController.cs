using AccessPointMap.API.Utility;
using AccessPointMap.Application.Integration.Wigle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public IntegrationCommandController(IWigleIntegrationService wigleIntegrationService)
        {
            _wigleIntegrationService = wigleIntegrationService ??
                throw new ArgumentNullException(nameof(wigleIntegrationService));
        }

        [HttpPost("wigle")]
        public async Task<IActionResult> CreateFromWigle([FromForm] IFormFile csvDatabaseFile) =>
            await RequestHandler.IntegrationServiceCommand(new Requests.Create { CsvDatabaseFile = csvDatabaseFile }, _wigleIntegrationService.Create);
    }
}
