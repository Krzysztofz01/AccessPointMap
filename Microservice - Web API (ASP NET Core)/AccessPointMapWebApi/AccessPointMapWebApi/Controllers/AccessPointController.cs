using AccessPointMapWebApi.Models;
using AccessPointMapWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccessPointMapWebApi.Controllers
{
    [Route("projects/accesspointmap/api/accesspoints/master")]
    [ApiController]
    public class AccessPointController : ControllerBase
    {
        private readonly IAccessPointRepository accessPointRepository;

        public AccessPointController(
            IAccessPointRepository accessPointRepository)
        {
            this.accessPointRepository = accessPointRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Accesspoint>>> GetAllAccesspoints()
        {
            return Ok(await accessPointRepository.GetAll());
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Accesspoint>>> GetAllAccesspointsAdmin()
        {
            return Ok(await accessPointRepository.GetAllAdmin());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Read,Admin")]
        public async Task<ActionResult<Accesspoint>> GetAccesspointById(int id)
        {
            var accesspoint = await accessPointRepository.GetById(id);
            if (accesspoint == null)
            {
                return NotFound();
            }
            return Ok(accesspoint);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Read,Admin")]
        public async Task<ActionResult<IEnumerable<Accesspoint>>> SearchAccesspoints([FromQuery] string ssid = null, [FromQuery] int freq = 0, [FromQuery] string brand = null, [FromQuery] string security = null)
        {
            var accesspoints = await accessPointRepository.SearchByParams(ssid, freq, brand, security);
            if (accesspoints == null)
            {
                return NotFound();
            }
            return Ok(accesspoints);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddOrUpdateAccesspoints(List<Accesspoint> accesspoints)
        {
            int rowsAffected = await accessPointRepository.AddOrUpdate(accesspoints);
            return Ok(new { rowsPosted = accesspoints.Count, rowsAffected });
        }

        [HttpPost("visibility")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DisplayAccesspoint(AccessPointDisplayDto displayDto)
        {
            if (await accessPointRepository.ChangeVisibility(displayDto.Id, displayDto.Display))
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPost("merge")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> MergeAccesspoints(List<int> accesspointsId)
        {
            if (await accessPointRepository.Merge(accesspointsId))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<AccessPointBrandCountDto>>> GetBrands()
        {
            return Ok(await accessPointRepository.GetBrandListOrderedCount());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Accesspoint>> DeleteAccesspoint(int id)
        {
            if (await accessPointRepository.Delete(id))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
