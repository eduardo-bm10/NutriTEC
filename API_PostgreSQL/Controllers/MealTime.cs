using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgre_API.Models;

namespace Postgre_API.Controllers
{
    /// <summary>
    /// Controller for managing meal times.
    /// </summary>
    [Route("api/Mealtime")]
    [ApiController]
    public class MealTimesController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="MealTimesController"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public MealTimesController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all meal times.
        /// </summary>
        /// <returns>A list of meal times.</returns>
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

        /// <summary>
        /// Retrieves a specific meal time by ID.
        /// </summary>
        /// <param name="id">The ID of the meal time.</param>
        /// <returns>The requested meal time.</returns>
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

        /// <summary>
        /// Creates a new meal time.
        /// </summary>
        /// <param name="mealTime">The meal time to create.</param>
        /// <returns>The created meal time.</returns>
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

        /// <summary>
        /// Updates an existing meal time.
        /// </summary>
        /// <param name="id">The ID of the meal time to update.</param>
        /// <param name="updatedMealTime">The updated meal time.</param>
        /// <returns>An action result indicating the status of the update operation.</returns>
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

        /// <summary>
        /// Deletes a meal time by ID.
        /// </summary>
        /// <param name="id">The ID of the meal time to delete.</param>
        /// <returns>An action result indicating the status of the delete operation.</returns>
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
