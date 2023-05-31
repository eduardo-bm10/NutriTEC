using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgre_API.Models;

namespace Postgre_API.Controllers
{
    [Route("api/Plans")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        public PlansController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Plans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plan>>> GetPlans()
        {
            return await _dbContext.Plans.ToListAsync();
        }

        // GET: api/Plans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plan>> GetPlan(int id)
        {
            var plan = await _dbContext.Plans.FindAsync(id);

            if (plan == null)
            {
                return NotFound();
            }

            return plan;
        }

        // POST: api/Plans
        [HttpPost]
        public async Task<ActionResult<Plan>> CreatePlan(string description, string nutritionistId, int mealtimeId, int productBarcode)
        {
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
            var Mealtime_Product = new MealtimeProduct
            {
                Mealtimeid = mealtimeId,
                ProductBarcode = productBarcode
            };
            _dbContext.Plans.Add(plan);
            _dbContext.MealtimeProducts.Add(Mealtime_Product);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlan), new { id = plan.Id }, plan);
        }

        // PUT: api/Plans/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlan(int id, Plan plan)
        {
            if (id != plan.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(plan).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanExists(id))
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

        // DELETE: api/Plans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlan(int id)
        {
            var plan = await _dbContext.Plans.FindAsync(id);

            if (plan == null)
            {
                return NotFound();
            }

            _dbContext.Plans.Remove(plan);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool PlanExists(int id)
        {
            return _dbContext.Plans.Any(e => e.Id == id);
        }
    }
}
