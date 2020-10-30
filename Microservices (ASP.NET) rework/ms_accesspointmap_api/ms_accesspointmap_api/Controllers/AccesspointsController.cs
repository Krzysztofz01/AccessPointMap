using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ms_accesspointmap_api.Models;
using ms_accesspointmap_api.Repositories;

namespace ms_accesspointmap_api.Controllers
{
    [Route("projects/accesspointmap/api/accesspoints/master")]
    [ApiController]
    public class AccesspointsController : ControllerBase
    {
        private readonly IAccessPointsRepository accessPointsRepository;

        public AccesspointsController(
            IAccessPointsRepository accessPointsRepository)
        {
            this.accessPointsRepository = accessPointsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Accesspoints>>> GetAccesspoints()
        {
            return Ok(await accessPointsRepository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Accesspoints>> GetAccesspoints(int id)
        {
            var accesspoint = await accessPointsRepository.GetById(id);
            if (accesspoint == null)
            {
                return NotFound();
            }

            return accesspoint;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Accesspoints>>> SearchAccesspoints([FromQuery] string ssid = null, [FromQuery] int freq = 0, [FromQuery] string brand = null, [FromQuery] string security = null)
        {
            var accesspoints = await accessPointsRepository.SearchByParams(ssid, freq, brand, security);
            if(accesspoints == null)
            {
                return NotFound();
            }

            return Ok(accesspoints);
        }

        [HttpPost]
        public async Task<ActionResult> PostAccesspoints(List<Accesspoints> accesspoints)
        {
            await accessPointsRepository.AddOrUpdate(accesspoints);
            int rowsAffected = await accessPointsRepository.SaveChanges();

            return Ok(new { rowsPosted = accesspoints.Count, rowsAffected });
        }

        [HttpPost("visibility")]
        public async Task<ActionResult> VisibilityAccesspoints(Visibility visibility)
        {
            await accessPointsRepository.ChangeVisibility(visibility.Id, visibility.Visible);
            int rowsAffected = await accessPointsRepository.SaveChanges();

            return Ok();
        }

        [HttpPost("merge")]
        public async Task<ActionResult> MergeAccesspoints(List<GuestAccesspoints> accesspoints)
        {
            await accessPointsRepository.Merge(accesspoints);
            int rowsAffected = await accessPointsRepository.SaveChanges();

            return Ok(new { rowsPosted = accesspoints.Count, rowsAffected });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Accesspoints>> DeleteAccesspoints(int id)
        {
            await accessPointsRepository.Delete(id);
            int rowsAffected = await accessPointsRepository.SaveChanges();

            if(rowsAffected < 1)
            {
                return BadRequest();
            }
            return Ok();
        }
    }

    public class Visibility
    {
        public int Id { get; set; }
        public bool Visible { get; set; }
    }
}
