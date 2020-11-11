using AccessPointMapWebApi.Models;
using AccessPointMapWebApi.Repositories;
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

        public GuestAccessPointController(
            IGuestAccesspointRepository guestAccesspointsRepository)
        {
            this.guestAccesspointRepository = guestAccesspointsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GuestAccesspoint>>> GetAllGuestAccesspoints()
        {
            return Ok(await guestAccesspointRepository.GetAll());
        }

        [HttpGet("{id}")]
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
        public async Task<ActionResult> AddGuestAccesspoints(List<GuestAccesspoint> accesspoints)
        {
            int rowsAffected = await guestAccesspointRepository.Add(accesspoints);
            return Ok(new { rowsAffected });
        }

        [HttpDelete("{id}")]
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
