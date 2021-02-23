using AccessPointMapWebApi.Models;
using AccessPointMapWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMapWebApi.Controllers
{
    [Route("projects/accesspointmap/api/accesspoints/master")]
    [ApiController]
    public class AccessPointController : ControllerBase
    {
        private readonly IAccessPointRepository accessPointRepository;
        private readonly ILogsRepository logsRepository;

        public AccessPointController(
            IAccessPointRepository accessPointRepository,
            ILogsRepository logsRepository)
        {
            this.accessPointRepository = accessPointRepository;
            this.logsRepository = logsRepository;
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

        [HttpGet("bssid/{bssid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Accesspoint>> GetAccesspointByBssid(string bssid)
        {
            var accesspoint = await accessPointRepository.GetByBssid(bssid);
            if (accesspoint == null)
            {
                return NotFound();
            }
            return Ok(accesspoint);
        }

        [HttpGet("check")]
        public async Task<ActionResult<IEnumerable<Accesspoint>>> SearchBySsid([FromQuery]string ssid)
        {
            if(!string.IsNullOrEmpty(ssid))
            {
                var ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                var accesspoints = await accessPointRepository.SearchBySsid(ssid);
                if (!accesspoints.Any())
                {
                    await logsRepository.Create($"{ipAddress} searched for {ssid} and found no results");
                    return NotFound();
                }

                await logsRepository.Create($"{ipAddress} searched for {ssid} and found some results");
                return Ok(accesspoints);
            }
            return BadRequest();
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
            if(accesspoints.Count > 0)
            {
                int rowsAffected = await accessPointRepository.AddOrUpdate(accesspoints);
                await logsRepository.Create($"{ accesspoints[0]?.PostedBy } commited { rowsAffected } updated to master table");
                return Ok(new { rowsPosted = accesspoints.Count, rowsAffected });
            }
            return BadRequest();
            
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
                await logsRepository.Create($"Merged { accesspointsId.Count } entities from queue to master table");
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("merge/all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> MergeAllAccesspoints()
        {
            if(await accessPointRepository.MergeAll())
            {
                await logsRepository.Create($"Merged all entities from queue to master table");
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
