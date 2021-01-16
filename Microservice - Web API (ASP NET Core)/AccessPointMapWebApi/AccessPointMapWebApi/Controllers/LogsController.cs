using System.Collections.Generic;
using System.Threading.Tasks;
using AccessPointMapWebApi.Models;
using AccessPointMapWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessPointMapWebApi.Controllers
{
    [Route("projects/accesspointmap/api/logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogsRepository logsRepository;

        public LogsController(ILogsRepository logsRepository)
        {
            this.logsRepository = logsRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Log>>> GetLogs()
        {
            return Ok(await logsRepository.GetLogs());
        }
    }
}
