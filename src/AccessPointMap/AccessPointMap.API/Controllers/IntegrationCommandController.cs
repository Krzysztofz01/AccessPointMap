using AccessPointMap.API.Controllers.Base;
using Aircrackng = AccessPointMap.Application.Integration.Aircrackng;
using Wigle = AccessPointMap.Application.Integration.Wigle;
using Wireshark = AccessPointMap.Application.Integration.Wireshark;
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
        private readonly Aircrackng.IAircrackngIntegrationService _aircrackngIntegrationService;
        private readonly Wireshark.IWiresharkIntegrationService _wiresharkIntegrationService;

        public IntegrationCommandController(
            Wigle.IWigleIntegrationService wigleIntegrationService,
            Aircrackng.IAircrackngIntegrationService aircrackngIntegrationService,
            Wireshark.IWiresharkIntegrationService wiresharkIntegrationService) : base()
        {
            _wigleIntegrationService = wigleIntegrationService ??
                throw new ArgumentNullException(nameof(wigleIntegrationService));

            _aircrackngIntegrationService = aircrackngIntegrationService ??
                throw new ArgumentNullException(nameof(aircrackngIntegrationService));

            _wiresharkIntegrationService = wiresharkIntegrationService ??
                throw new ArgumentNullException(nameof(wiresharkIntegrationService));
        }

        [HttpPost("wigle/csv")]
        public async Task<IActionResult> CreateFromWigle([FromForm] Wigle.Commands.CreateAccessPointsFromCsvFile command) =>
            await ExecuteService(command, _wigleIntegrationService.Handle);

        [HttpPost("wigle/csvgz")]
        public async Task<IActionResult> CreateFromWigle([FromForm] Wigle.Commands.CreateAccessPointsFromCsvGzFile command) =>
            await ExecuteService(command, _wigleIntegrationService.Handle);

        [HttpPost("aircrackng/csv")]
        public async Task<IActionResult> CreateAccessPointsFromCsvFile([FromForm] Aircrackng.Commands.CreateAccessPointsFromCsvFile command) =>
            await ExecuteService(command, _aircrackngIntegrationService.Handle);

        [HttpPost("aircrackng/cap")]
        public async Task<IActionResult> CreatePacketsFromPcapFile([FromForm] Aircrackng.Commands.CreatePacketsFromPcapFile command) =>
            await ExecuteService(command, _aircrackngIntegrationService.Handle);

        [HttpPost("wireshark/pcap")]
        public async Task<IActionResult> CreatePacketsFromPcapFile([FromForm] Wireshark.Commands.CreatePacketsFromPcapFile command) =>
            await ExecuteService(command, _wiresharkIntegrationService.Handle);
    }
}
