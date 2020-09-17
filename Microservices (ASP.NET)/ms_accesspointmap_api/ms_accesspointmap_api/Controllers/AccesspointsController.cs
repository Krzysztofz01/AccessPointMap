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
        private readonly AccessPointMapContext _context;

        public AccesspointsController(AccessPointMapContext context)
        {
            _context = context;
        }

        // GET: api/Accesspoints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Accesspoints>>> GetAccesspoints()
        {
            return await _context.Accesspoints.ToListAsync();
        }

        // GET: api/Accesspoints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Accesspoints>> GetAccesspoints(int id)
        {
            var accesspoints = await _context.Accesspoints.FindAsync(id);

            if (accesspoints == null)
            {
                return NotFound();
            }

            return accesspoints;
        }

        // PUT: api/Accesspoints/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccesspoints(int id, Accesspoints accesspoints)
        {
            if (id != accesspoints.Id)
            {
                return BadRequest();
            }

            _context.Entry(accesspoints).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccesspointsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Accesspoints
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Accesspoints>> PostAccesspoints(Accesspoints accesspoints)
        {
            _context.Accesspoints.Add(accesspoints);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccesspoints", new { id = accesspoints.Id }, accesspoints);
        }

        // DELETE: api/Accesspoints/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Accesspoints>> DeleteAccesspoints(int id)
        {
            var accesspoints = await _context.Accesspoints.FindAsync(id);
            if (accesspoints == null)
            {
                return NotFound();
            }

            _context.Accesspoints.Remove(accesspoints);
            await _context.SaveChangesAsync();

            return accesspoints;
        }

        private bool AccesspointsExists(int id)
        {
            return _context.Accesspoints.Any(e => e.Id == id);
        }
    }
}
