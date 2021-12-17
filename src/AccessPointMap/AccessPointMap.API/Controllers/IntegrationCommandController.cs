using AccessPointMap.API.Controllers.Base;
using AccessPointMap.Application.Integration.Aircrackng
using AccessPointMap.Application.Integration.Wigle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccessPointMap.API.Controllers
{
    [Route("api/v{version:apiVersion}/integrations")]
    [ApiVersion("1.0")]
    public class IntegrationCommandController : CommandController
    {
        private readonly IWigleIntegrationService _wigleIntegrationService;
        private readonly IAircrackngIntegrationService _aircrackngIntegrationService;

        public IntegrationCommandController(IWigleIntegrationService wigleIntegrationService, IAircrackngIntegrationService aircrackngIntegrationService) : base()
        {
            _wigleIntegrationService = wigleIntegrationService ??
                throw new ArgumentNullException(nameof(wigleIntegrationService));
                
            _aircrackIntegrationService = aircrackngIntegrationService ??
                throw new ArgumentNullException(nameof(aircrackngIntegrationService));
        }

        [HttpPost("wigle")]
        public async Task<IActionResult> CreateFromWigle([FromForm] IFormFile csvDatabaseFile) =>
            await ExecuteService(new Requests.Create { CsvDatabaseFile = csvDatabaseFile }, _wigleIntegrationService.Create);
            
        [HttpPost("aircrackng")]
        public async Task<IActionResult> CreateFromAircrackng(Application.Integration.Aircrackng.Requests.Create request) =>
            await RequestHandler.IntegrationServiceCommand(request, _aircrackngIntegrationService.Create);
    }
}
