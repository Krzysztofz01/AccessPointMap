using AccessPointMap.API.Controllers.Base;
using AccessPointMap.Application.Core;
using AccessPointMap.Infrastructure.Core.Abstraction;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Wigle = AccessPointMap.Application.Integration.Wigle;

namespace AccessPointMap.API.Controllers
{
    [Route("api/v{version:apiVersion}/integrations")]
    [ApiVersion("1.0")]
    public class IntegrationQueryController : QueryController
    {
        private readonly Wigle.IWigleIntegrationService _wigleIntegrationService;

        private const string CsvContentMimeType = "text/csv";

        public IntegrationQueryController(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache, ILogger<IntegrationQueryController> logger, Wigle.IWigleIntegrationService wigleIntegrationService) : base(unitOfWork, mapper, memoryCache, logger)
        {
            _wigleIntegrationService = wigleIntegrationService ??
                throw new ArgumentNullException(nameof(wigleIntegrationService));
        }

        [HttpGet("wigle/csv")]
        [Produces(CsvContentMimeType)]
        public async Task<IActionResult> DownloadWigleCsv(
            CancellationToken cancellationToken)
        {
            var result = await _wigleIntegrationService.HandleQueryAsync(new Wigle.Queries.ExportAccessPointsToCsv(), cancellationToken);

            // TODO: Error message
            if (result.IsFailure) return StatusCode((int)HttpStatusCode.InternalServerError);

            return await HandleFileResult(result.Value as ExportFile, true, CsvContentMimeType, cancellationToken);
        }
    }
}
