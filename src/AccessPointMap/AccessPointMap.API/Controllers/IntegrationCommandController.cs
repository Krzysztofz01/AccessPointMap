using AccessPointMap.API.Controllers.Base;
using AccessPointMap.Application.Integration.Aircrackng;
using Wigle = AccessPointMap.Application.Integration.Wigle;
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
        private readonly Wigle.IWigleIntegrationService _wigleIntegrationService;
        private readonly IAircrackngIntegrationService _aircrackngIntegrationService;

        public IntegrationCommandController(Wigle.IWigleIntegrationService wigleIntegrationService, IAircrackngIntegrationService aircrackngIntegrationService) : base()
        {
            _wigleIntegrationService = wigleIntegrationService ??
                throw new ArgumentNullException(nameof(wigleIntegrationService));

            _aircrackngIntegrationService = aircrackngIntegrationService ??
                throw new ArgumentNullException(nameof(aircrackngIntegrationService));
        }

        [HttpPost("wigle/csv")]
        public async Task<IActionResult> CreateFromWigle(Wigle.Commands.CreateAccessPointsFromCsvFile command) =>
            await ExecuteService(command, _wigleIntegrationService.Handle);

        [HttpPost("aircrackng")]
        public async Task<IActionResult> CreateFromAircrackng([FromForm] IFormFile csvDumpFile) =>
            await ExecuteService(new Application.Integration.Aircrackng.Requests.Create { CsvDumpFile = csvDumpFile }, _aircrackngIntegrationService.Create);
    }
}
