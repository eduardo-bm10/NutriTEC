
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgre_API.Models;

namespace Postgre_API.Controllers
{
    [Route("api/Mealtime")]
    [ApiController]
    public class MealTimesController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        public MealTimesController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealTime>>> GetMealTimes()
        {
            return await _dbContext.MealTimes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MealTime>> GetMealTime(int id)
        {
            var mealTime = await _dbContext.MealTimes.FindAsync(id);

            if (mealTime == null)
            {
                return NotFound();
            }

            return mealTime;
        }

        [HttpPost]
        public async Task<ActionResult<MealTime>> CreateMealTime(MealTime mealTime)
        {
            _dbContext.MealTimes.Add(mealTime);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMealTime), new { id = mealTime.Id }, mealTime);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMealTime(int id, MealTime updatedMealTime)
        {
            var mealTime = await _dbContext.MealTimes.FindAsync(id);

            if (mealTime == null)
            {
                return NotFound();
            }

            mealTime.Name = updatedMealTime.Name;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMealTime(int id)
        {
            var mealTime = await _dbContext.MealTimes.FindAsync(id);

            if (mealTime == null)
            {
                return NotFound();
            }

            _dbContext.MealTimes.Remove(mealTime);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
