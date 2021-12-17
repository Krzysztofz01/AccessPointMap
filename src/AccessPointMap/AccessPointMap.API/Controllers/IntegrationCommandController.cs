using AccessPointMap.API.Controllers.Base;
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

        public IntegrationCommandController(IWigleIntegrationService wigleIntegrationService) : base()
        {
            _wigleIntegrationService = wigleIntegrationService ??
                throw new ArgumentNullException(nameof(wigleIntegrationService));
        }

        [HttpPost("wigle")]
        public async Task<IActionResult> CreateFromWigle([FromForm] IFormFile csvDatabaseFile) =>
            await ExecuteService(new Requests.Create { CsvDatabaseFile = csvDatabaseFile }, _wigleIntegrationService.Create);
    }
}
