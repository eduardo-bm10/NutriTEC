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
    [Route("api/Consumptions")]
    [ApiController]
    public class ConsumptionsController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        public ConsumptionsController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

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

        [HttpPost("post/{patientId}/{date}/{mealtimeId}/{productBarcode}"))]
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
