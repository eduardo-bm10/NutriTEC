using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Postgre_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Postgre_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanMealtimeAssociationsController : ControllerBase
    {
        private readonly NutritecDbContext _context;

        public PlanMealtimeAssociationsController(NutritecDbContext context)
        {
            _context = context;
        }

        // GET: api/PlanMealtimeAssociations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanMealtimeAssociation>>> GetPlanMealtimeAssociations()
        {
            try{
            return await _context.PlanMealtimeAssociations.ToListAsync();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        // GET: api/PlanMealtimeAssociations/5
        [HttpGet("{planid}/{mealtimeid}")]
        public async Task<ActionResult<PlanMealtimeAssociation>> GetPlanMealtimeAssociation(int planid, int mealtimeid)
        {
            var planMealtimeAssociation = await _context.PlanMealtimeAssociations.FindAsync(planid, mealtimeid);

            if (planMealtimeAssociation == null)
            {
                return NotFound(new { message = "PlanMealtimeAssociation not found" });
            }

            return planMealtimeAssociation;
        }

        // POST: api/PlanMealtimeAssociations
        [HttpPost]
        public async Task<ActionResult<PlanMealtimeAssociation>> CreatePlanMealtimeAssociation(PlanMealtimeAssociation planMealtimeAssociation)
        {
            _context.PlanMealtimeAssociations.Add(planMealtimeAssociation);
            await _context.SaveChangesAsync();

            return Ok(new { message = "ok" });
        }

        // PUT: api/PlanMealtimeAssociations/5
        [HttpPut("{planid}/{mealtimeid}")]
        public async Task<IActionResult> UpdatePlanMealtimeAssociation(int planid, int mealtimeid, PlanMealtimeAssociation planMealtimeAssociation)
        {
            try{
            if (planid != planMealtimeAssociation.Planid || mealtimeid != planMealtimeAssociation.Mealtimeid)
            {
                return BadRequest(new { message = "PlanMealtimeAssociation not found" });
            }

            _context.Entry(planMealtimeAssociation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanMealtimeAssociationExists(planid, mealtimeid))
                {
                    return NotFound(new { message = "PlanMealtimeAssociation not found" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        // DELETE: api/PlanMealtimeAssociations/5
        [HttpDelete("{planid}/{mealtimeid}")]
        public async Task<IActionResult> DeletePlanMealtimeAssociation(int planid, int mealtimeid)
        {
            try{
            var planMealtimeAssociation = await _context.PlanMealtimeAssociations.FindAsync(planid, mealtimeid);
            if (planMealtimeAssociation == null)
            {
                return NotFound(new { message = "PlanMealtimeAssociation not found" });
            }

            _context.PlanMealtimeAssociations.Remove(planMealtimeAssociation);
            await _context.SaveChangesAsync();

            return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        private bool PlanMealtimeAssociationExists(int planid, int mealtimeid)
        {
            return _context.PlanMealtimeAssociations.Any(e => e.Planid == planid && e.Mealtimeid == mealtimeid);
        }
    }
}
