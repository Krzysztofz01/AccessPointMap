using AccessPointMap.API.Controllers.Base;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Wigle = AccessPointMap.Application.Integration.Wigle;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using System.IO;

namespace AccessPointMap.API.Controllers
{
    [Route("api/v{version:apiVersion}/integrations")]
    [ApiVersion("1.0")]
    public class IntegrationQueryController : QueryController
    {
        private readonly Wigle.IWigleIntegrationService _wigleIntegrationService;

        private const string _csvContentType = "text/csv";

        public IntegrationQueryController(IDataAccess dataAccess, IMapper mapper, IMemoryCache memoryCache, Wigle.IWigleIntegrationService wigleIntegrationService) : base(dataAccess, mapper, memoryCache)
        {
            _wigleIntegrationService = wigleIntegrationService ??
                throw new ArgumentNullException(nameof(wigleIntegrationService));
        }

        [HttpGet("wigle/csv")]
        public async Task<IActionResult> DownloadWigleCsv()
        {
            var response = await _wigleIntegrationService.Query(new Wigle.Queries.ExportAccessPointsToCsv());

            return MapToFile((byte[])response, _csvContentType);
        }
    }
}
