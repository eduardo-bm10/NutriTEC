using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgre_API.Models;
using Newtonsoft.Json;

namespace Postgre_API.Controllers
{

    /// <summary>
    /// Controller for managing consumptions.
    /// </summary>
    [Route("api/Consumptions")]
    [ApiController]
    public class ConsumptionsController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumptionsController"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ConsumptionsController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all consumptions.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consumption>>> GetConsumptions()
        {
            try
            {
                return await _dbContext.Consumptions.ToListAsync();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Retrieves a specific consumption by patient ID, date, and mealtime ID.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <param name="date">The date of the consumption.</param>
        /// <param name="mealtimeId">The ID of the mealtime.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("{patientId}/{date}/{mealtimeId}")]
        public async Task<ActionResult<Consumption>> GetConsumption(string patientId, DateTime date, int mealtimeId)
        {
            try
            {
                var consumption = await _dbContext.Consumptions.FindAsync(patientId, date);

                if (consumption == null)
                {
                    return NotFound(new { message = "Consumption not found" });
                }

                return consumption;
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Creates a new consumption.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <param name="date">The date of the consumption.</param>
        /// <param name="mealtimeId">The ID of the mealtime.</param>
        /// <param name="productBarcode">The barcode of the product.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("post/{patientId}/{date}/{mealtimeId}/{productBarcode}")]
        public async Task<ActionResult<Consumption>> CreateConsumption(string patientId, DateTime date, int mealtimeId, int productBarcode)
        {
            try
            {
                var patientId_exists = await _dbContext.Patients.FindAsync(patientId);
                var mealtimeId_exists = await _dbContext.MealTimes.FindAsync(mealtimeId);
                var productBarcode_exists = await _dbContext.Products.FindAsync(productBarcode);

                if (patientId_exists == null)
                {
                    return NotFound(new { message = "Patient not found" });
                }
                else if (mealtimeId_exists == null)
                {
                    return NotFound(new { message = "Mealtime not found" });
                }
                else if (productBarcode_exists == null)
                {
                    return NotFound(new { message = "Product not found" });
                }

                var consumption = new Consumption
                {
                    Patientid = patientId,
                    Date = new DateOnly(date.Year, date.Month, date.Day),
                    Mealtime = mealtimeId,
                    Productbarcode = productBarcode
                };

                _dbContext.Consumptions.Add(consumption);
                await _dbContext.SaveChangesAsync();

                var options = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                string json = JsonConvert.SerializeObject(consumption, options);

                return Ok(new { message = json});
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Updates an existing consumption.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <param name="date">The date of the consumption.</param>
        /// <param name="mealtimeId">The ID of the mealtime.</param>
        /// <param name="productBarcode">The barcode of the product.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPut("{patientId}/{date}/{mealtimeId}")]
        public async Task<IActionResult> UpdateConsumption(string patientId, DateTime date, int productBarcode, int mealtimeId)
        {
            try
            {
                var consumption = await _dbContext.Consumptions.FindAsync(patientId, new DateOnly(date.Year, date.Month, date.Day), mealtimeId);

                if (consumption == null)
                {
                    return NotFound(new { message = "Consumption not found" });
                }

                consumption.Mealtime = mealtimeId;
                consumption.Productbarcode = productBarcode;

                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Deletes a consumption.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <param name="date">The date of the consumption.</param>
        /// <param name="mealtimeId">The ID of the mealtime.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpDelete("{patientId}/{date}/{mealtimeId}")]
        public async Task<IActionResult> DeleteConsumption(string patientId, DateTime date, int mealtimeId)
        {
            try
            {
                var consumption = await _dbContext.Consumptions.FindAsync(patientId, new DateOnly(date.Year, date.Month, date.Day), mealtimeId);

                if (consumption == null)
                {
                    return NotFound(new { message = "Consumption not found" });
                }

                _dbContext.Consumptions.Remove(consumption);
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
