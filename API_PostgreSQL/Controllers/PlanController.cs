using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgre_API.Models;
using Newtonsoft.Json;

namespace Postgre_API.Controllers
{
    [Route("api/Plan")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        public PlansController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all plans.
        /// </summary>
        /// <returns>A list of plans.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plan>>> GetPlans()
        {
            try{
            return await _dbContext.Plans.ToListAsync();
            }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}

        /// <summary>
        /// Retrieves a specific plan by its ID.
        /// </summary>
        /// <param name="id">The ID of the plan.</param>
        /// <returns>The plan with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Plan>> GetPlan(int id)
        {
            try{
                var plan = await _dbContext.Plans.FindAsync(id);

                if (plan == null){
                    return NotFound(new {message = "Plan not found"});
                }

                return plan;
            }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }
        }

        /// <summary>
        /// Creates a new plan.
        /// </summary>
        /// <param name="description">The description of the plan.</param>
        /// <param name="nutritionistId">The ID of the nutritionist associated with the plan.</param>
        /// <param name="mealtimeId">The ID of the mealtime associated with the plan.</param>
        /// <param name="productBarcode">The barcode of the product associated with the plan.</param>
        /// <returns>The created plan.</returns>
        [HttpPost("{description}")]
        public async Task<ActionResult<Plan>> CreatePlan(string description, string nutritionistId, int mealtimeId, int productBarcode)
        {
            try{
            var mealtime_exists = await _dbContext.MealTimes.FindAsync(mealtimeId);
            var nutritionistId_exists = await _dbContext.Nutritionists.FindAsync(nutritionistId);
            var productBarcode_exists = await _dbContext.Products.FindAsync(productBarcode);
            if(mealtime_exists == null){
                return NotFound("Mealtime not found");
            }else if(nutritionistId_exists == null){
                return NotFound("Nutritionist not found");
            }else if(productBarcode_exists == null){
                return NotFound("Product not found");
            }
            var plan = new Plan
            {
                Nutritionistid = nutritionistId,
                Description = description
            };
            _dbContext.Plans.Add(plan);
            await _dbContext.SaveChangesAsync();
            var thePlan = await _dbContext.Plans.FirstOrDefaultAsync(p => p.Description == description);
            var PlanMealtimeAssociation = new PlanMealtimeAssociation
            {
                Planid = thePlan.Id,
                Mealtimeid = mealtimeId
            };
            _dbContext.PlanMealtimeAssociations.Add(PlanMealtimeAssociation);
            
            await _dbContext.SaveChangesAsync();

           var options = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(plan, options);
           return Ok(json);

        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}

        
        /// <summary>
        /// Updates a specific plan.
        /// </summary>
        /// <param name="id">The ID of the plan to update.</param>
        /// <param name="plan">The updated plan object.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlan(int id, Plan plan)
        {
            try{
            if (id != plan.Id)
            {
                return NotFound(new { message = "not found" });
            }

            _dbContext.Entry(plan).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();


            return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Deletes a specific plan.
        /// </summary>
        /// <param name="id">The ID of the plan to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlan(int id)
        {
            try{
            var plan = await _dbContext.Plans.FindAsync(id);

            if (plan == null)
            {
                return NotFound(new { message = "Plan not found" });
            }

            _dbContext.Plans.Remove(plan);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        private bool PlanExists(int id)
        {
            return _dbContext.Plans.Any(e => e.Id == id);
        }
    }
}
