using AccessPointMap.API.Controllers.Base;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

using Aircrackng = AccessPointMap.Application.Integration.Aircrackng;
using Wigle = AccessPointMap.Application.Integration.Wigle;
using Wireshark = AccessPointMap.Application.Integration.Wireshark;

namespace AccessPointMap.API.Controllers
{
    [Route("api/v{version:apiVersion}/integrations")]
    [ApiVersion("1.0")]
    public class IntegrationCommandController : CommandController<IntegrationCommandController>
    {
        private readonly Wigle.IWigleIntegrationService _wigleIntegrationService;
        private readonly Aircrackng.IAircrackngIntegrationService _aircrackngIntegrationService;
        private readonly Wireshark.IWiresharkIntegrationService _wiresharkIntegrationService;

        public IntegrationCommandController(
            ILogger<IntegrationCommandController> logger,
            Wigle.IWigleIntegrationService wigleIntegrationService,
            Aircrackng.IAircrackngIntegrationService aircrackngIntegrationService,
            Wireshark.IWiresharkIntegrationService wiresharkIntegrationService) : base(logger)
        {
            _wigleIntegrationService = wigleIntegrationService ??
                throw new ArgumentNullException(nameof(wigleIntegrationService));

            _aircrackngIntegrationService = aircrackngIntegrationService ??
                throw new ArgumentNullException(nameof(aircrackngIntegrationService));

            _wiresharkIntegrationService = wiresharkIntegrationService ??
                throw new ArgumentNullException(nameof(wiresharkIntegrationService));
        }

        [HttpPost("wigle/csv")]
        public async Task<IActionResult> CreateFromWigle(
            [FromForm] Wigle.Commands.CreateAccessPointsFromCsvFile command,
            CancellationToken cancellationToken) =>
                await ExecuteIntegrationCommandAsync(command, _wigleIntegrationService, cancellationToken);

        [HttpPost("wigle/csvgz")]
        public async Task<IActionResult> CreateFromWigle(
            [FromForm] Wigle.Commands.CreateAccessPointsFromCsvGzFile command,
            CancellationToken cancellationToken) =>
                await ExecuteIntegrationCommandAsync(command, _wigleIntegrationService, cancellationToken);

        [HttpPost("aircrackng/csv")]
        public async Task<IActionResult> CreateAccessPointsFromCsvFile(
            [FromForm] Aircrackng.Commands.CreateAccessPointsFromCsvFile command,
            CancellationToken cancellationToken = default) =>
                await ExecuteIntegrationCommandAsync(command, _aircrackngIntegrationService, cancellationToken);

        [HttpPost("aircrackng/cap")]
        public async Task<IActionResult> CreatePacketsFromPcapFile(
            [FromForm] Aircrackng.Commands.CreatePacketsFromPcapFile command,
            CancellationToken cancellationToken) =>
                await ExecuteIntegrationCommandAsync(command, _aircrackngIntegrationService, cancellationToken);

        [HttpPost("wireshark/pcap")]
        public async Task<IActionResult> CreatePacketsFromPcapFile(
            [FromForm] Wireshark.Commands.CreatePacketsFromPcapFile command,
            CancellationToken cancellationToken) =>
                await ExecuteIntegrationCommandAsync(command, _wiresharkIntegrationService, cancellationToken);
    }
}
