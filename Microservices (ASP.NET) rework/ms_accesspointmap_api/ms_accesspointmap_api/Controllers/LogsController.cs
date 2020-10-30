using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ms_accesspointmap_api.Models;
using ms_accesspointmap_api.Repositories;

namespace ms_accesspointmap_api.Controllers
{
    [Route("projects/accesspointmap/api/logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogsRepository logsRepository;

        public LogsController(
            ILogsRepository logsRepository)
        {
            this.logsRepository = logsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Logs>>> GetLogs()
        {
            return Ok(await logsRepository.GetAll());
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Logs>>> SearchLogs([FromQuery] int id, [FromQuery] string status, [FromQuery] string endpoint, [FromQuery] string dateTime)
        {
            return Ok(await logsRepository.Search(id, status, endpoint, DateTime.Parse(dateTime)));
        }

        [HttpPost]
        public async Task<ActionResult> PostLogs(Logs logs)
        {
            await logsRepository.Add(logs);
            await logsRepository.SaveChanges();

            return Ok();
        }
    }
}
