using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Read,ReadWrite,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Accesspoints>>> GetAccesspoints()
        {
            return Ok(await accessPointsRepository.GetAll());
        }

        [Authorize(Roles = "Read,ReadWrite,Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Accesspoints>> GetAccesspoints(int id)
        {
            var accesspoint = await accessPointsRepository.GetById(id);
            if (accesspoint == null)
            {
                return NotFound();
            }
            return Ok(accesspoint);
        }

        [Authorize(Roles = "Read,ReadWrite,Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> PostAccesspoints(List<Accesspoints> accesspoints)
        {
            int rowsAffected = await accessPointsRepository.AddOrUpdate(accesspoints);
            return Ok(new { rowsPosted = accesspoints.Count, rowsAffected});
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("visibility")]
        public async Task<ActionResult> VisibilityAccesspoints(Visibility visibility)
        {
            if(await accessPointsRepository.ChangeVisibility(visibility.Id, visibility.Visible))
            {
                return Ok();
            }
            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("merge")]
        public async Task<ActionResult> MergeAccesspoints(List<int> accesspointsId)
        {
            if(await accessPointsRepository.Merge(accesspointsId))
            {
                return Ok();
            }
            return BadRequest();
        }

        [Authorize(Roles = "Read,ReadWrite,Admin")]
        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<string>>> GetBrands()
        {
            return Ok(await accessPointsRepository.GetBrandList());
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Accesspoints>> DeleteAccesspoints(int id)
        {
            if(await accessPointsRepository.Delete(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }

    public class Visibility
    {
        public int Id { get; set; }
        public bool Visible { get; set; }
    }
}
