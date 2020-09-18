using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ms_accesspointmap_api.Models;

namespace ms_accesspointmap_api.Controllers
{
    [Route("projects/accesspointmap/api/[controller]")]
    [ApiController]
    public class AccesspointsController : ControllerBase
    {
        private IAccesspointsRepository accesspointsRepository;

        public AccesspointsController(IAccesspointsRepository accesspointsRepository)
        {
            this.accesspointsRepository = accesspointsRepository;
        }

        // GET: projects/accesspointmap/api/Accesspoints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Accesspoints>>> GetAccesspoints()
        {
            var accesspoints = await accesspointsRepository.GetAccesspoints();

            if(!accesspoints.Any())
            {
                return NotFound();
                
            }
            return Ok(accesspoints);
        }

        // GET: projects/accesspointmap/api/Accesspoints/id/5
        [HttpGet("id/{id}")]
        public async Task<ActionResult<Accesspoints>> GetAccesspoints(int id)
        {
            var accesspoint = await accesspointsRepository.GetAccesspointById(id);

            if (accesspoint == null)
            {
                return NotFound();
            }

            return Ok(accesspoint);
        }

        // POST: api/Accesspoints
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult> PostAccesspoints(List<Accesspoints> accesspoints)
        {
            foreach(Accesspoints element in accesspoints)
            {
                if(ModelState.IsValid)
                {
                    await accesspointsRepository.CreateOrUpdate(element);
                }
            }

            int rowsAffected = await accesspointsRepository.Save();
            return Ok(new { rowsAffected });
        }

        // GET: projects/accesspointmap/api/Accesspoints/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Accesspoints>>> GetAccesspoints([FromQuery] string ssid = null, [FromQuery] int freq = 0, [FromQuery] string brand = null, [FromQuery] string security = null)
        {
            var accesspoints = await accesspointsRepository.SearchAccesspoints(ssid, freq, brand, security);
            
            if(!accesspoints.Any())
            {
                return NotFound();
            }

            return Ok(accesspoints);
        }
    }
}
