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

        /// <summary>
        /// Retrieves all plan mealtime associations.
        /// </summary>
        /// <returns>A list of plan mealtime associations.</returns>
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

        /// <summary>
        /// Retrieves a specific plan mealtime association by its plan ID and mealtime ID.
        /// </summary>
        /// <param name="planid">The ID of the plan.</param>
        /// <param name="mealtimeid">The ID of the mealtime.</param>
        /// <returns>The plan mealtime association with the specified plan ID and mealtime ID.</returns>
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

        /// <summary>
        /// Creates a new plan mealtime association.
        /// </summary>
        /// <param name="planMealtimeAssociation">The plan mealtime association to create.</param>
        /// <returns>The created plan mealtime association.</returns>
        [HttpPost]
        public async Task<ActionResult<PlanMealtimeAssociation>> CreatePlanMealtimeAssociation(PlanMealtimeAssociation planMealtimeAssociation)
        {
            _context.PlanMealtimeAssociations.Add(planMealtimeAssociation);
            await _context.SaveChangesAsync();

            return Ok(new { message = "ok" });
        }

        /// <summary>
        /// Updates a specific plan mealtime association.
        /// </summary>
        /// <param name="planid">The ID of the plan.</param>
        /// <param name="mealtimeid">The ID of the mealtime.</param>
        /// <param name="planMealtimeAssociation">The updated plan mealtime association object.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
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


        /// <summary>
        /// Deletes a specific plan mealtime association.
        /// </summary>
        /// <param name="planid">The ID of the plan.</param>
        /// <param name="mealtimeid">The ID of the mealtime.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
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
