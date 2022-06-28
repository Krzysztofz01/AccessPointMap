using AccessPointMap.API.Controllers.Base;
using Aircrackng = AccessPointMap.Application.Integration.Aircrackng;
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
        private readonly Aircrackng.IAircrackngIntegrationService _aircrackngIntegrationService;

        public IntegrationCommandController(IWigleIntegrationService wigleIntegrationService, Aircrackng.IAircrackngIntegrationService aircrackngIntegrationService) : base()
        {
            _wigleIntegrationService = wigleIntegrationService ??
                throw new ArgumentNullException(nameof(wigleIntegrationService));

            _aircrackngIntegrationService = aircrackngIntegrationService ??
                throw new ArgumentNullException(nameof(aircrackngIntegrationService));
        }

        [HttpPost("wigle")]
        public async Task<IActionResult> CreateFromWigle([FromForm] IFormFile csvDatabaseFile) =>
            await ExecuteService(new Application.Integration.Wigle.Requests.Create { CsvDatabaseFile = csvDatabaseFile }, _wigleIntegrationService.Create);

        [HttpPost("aircrackng/csv")]
        public async Task<IActionResult> CreateAccessPointsFromCsvFile(Aircrackng.Commands.CreateAccessPointsFromCsvFile command) =>
            await ExecuteService(command, _aircrackngIntegrationService.Handle);

        [HttpPost("aircrackng/cap")]
        public async Task<IActionResult> CreatePacketsFromPcapFile(Aircrackng.Commands.CreatePacketsFromPcapFile command) =>
            await ExecuteService(command, _aircrackngIntegrationService.Handle);
    }
}
