using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
            try
            {
                var mealTimes = await _dbContext.MealTimes.ToListAsync();
                return Ok(mealTimes);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MealTime>> GetMealTime(int id)
        {
            try
            {
                var mealTime = await _dbContext.MealTimes.FindAsync(id);

                if (mealTime == null)
                {
                    return NotFound(new { message = "MealTime not found" });
                }

                return Ok(mealTime);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<MealTime>> CreateMealTime(MealTime mealTime)
        {
            try
            {
                _dbContext.MealTimes.Add(mealTime);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetMealTime), new { id = mealTime.Id }, mealTime);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMealTime(int id, MealTime updatedMealTime)
        {
            try
            {
                var mealTime = await _dbContext.MealTimes.FindAsync(id);

                if (mealTime == null)
                {
                    return NotFound(new { message = "MealTime not found" });
                }

                mealTime.Name = updatedMealTime.Name;

                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMealTime(int id)
        {
            try
            {
                var mealTime = await _dbContext.MealTimes.FindAsync(id);

                if (mealTime == null)
                {
                    return NotFound(new { message = "MealTime not found" });
                }

                _dbContext.MealTimes.Remove(mealTime);
                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
