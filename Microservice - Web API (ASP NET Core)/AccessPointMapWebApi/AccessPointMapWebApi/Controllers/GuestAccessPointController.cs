using AccessPointMapWebApi.Models;
using AccessPointMapWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccessPointMapWebApi.Controllers
{
    [Route("projects/accesspointmap/api/accesspoints/queue")]
    [ApiController]
    public class GuestAccessPointController : ControllerBase
    {
        private readonly IGuestAccesspointRepository guestAccesspointRepository;
        private readonly ILogsRepository logsRepository;

        public GuestAccessPointController(
            IGuestAccesspointRepository guestAccesspointRepository,
            ILogsRepository logsRepository)
        {
            this.guestAccesspointRepository = guestAccesspointRepository;
            this.logsRepository = logsRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<GuestAccesspoint>>> GetAllGuestAccesspoints()
        {
            return Ok(await guestAccesspointRepository.GetAll());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GuestAccesspoint>> GetGuestAccesspointById(int id)
        {
            var accesspoint = await guestAccesspointRepository.GetById(id);
            if (accesspoint == null)
            {
                return NotFound();
            }
            return Ok(accesspoint);
        }

        [HttpPost]
        [Authorize(Roles = "Write,Admin")]
        public async Task<ActionResult> AddGuestAccesspoints(List<GuestAccesspoint> accesspoints)
        {
            if(accesspoints.Count > 0)
            {
                int rowsAffected = await guestAccesspointRepository.Add(accesspoints);
                await logsRepository.Create($"{ accesspoints[0]?.PostedBy } commited { rowsAffected } updates to the queue table");
                return Ok(new { rowsAffected });
            }
            return BadRequest();
            
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GuestAccesspoint>> DeleteGuestAccesspoint(int id)
        {
            if (await guestAccesspointRepository.Delete(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
