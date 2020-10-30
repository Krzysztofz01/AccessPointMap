using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ms_accesspointmap_api.Models;
using ms_accesspointmap_api.Repositories;

namespace ms_accesspointmap_api.Controllers
{
    [Route("projects/accesspointmap/api/accesspoints/queue")]
    [ApiController]
    public class GuestAccesspointsController : ControllerBase
    {
        private readonly IGuestAccesspointsRepository guestAccesspointsRepository;

        public GuestAccesspointsController(
            IGuestAccesspointsRepository guestAccesspointsRepository)
        {
            this.guestAccesspointsRepository = guestAccesspointsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GuestAccesspoints>>> GetGuestAccesspoints()
        {
            return Ok(await guestAccesspointsRepository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GuestAccesspoints>> GetGuestAccesspoints(int id)
        {
            var accesspoint = await guestAccesspointsRepository.GetById(id);
            if (accesspoint == null)
            {
                return NotFound();
            }

            return Ok(accesspoint);
        }

        [HttpPost]
        public async Task<ActionResult> PostGuestAccesspoints(List<GuestAccesspoints> accesspoints)
        {
            await guestAccesspointsRepository.AddOrUpdate(accesspoints);
            int rowsAffected = await guestAccesspointsRepository.SaveChanges();

            return Ok(new { rowsPosted = accesspoints.Count, rowsAffected });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GuestAccesspoints>> DeleteGuestAccesspoints(int id)
        {
            await guestAccesspointsRepository.Delete(id);
            int rowsAffected = await guestAccesspointsRepository.SaveChanges();
            
            if (rowsAffected < 1)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
