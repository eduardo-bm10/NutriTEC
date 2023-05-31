
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgre_API.Models;

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
            return await _dbContext.Consumptions.ToListAsync();
        }

        [HttpGet("{patientId}/{date}")]
        public async Task<ActionResult<Consumption>> GetConsumption(string patientId, DateTime date, int mealtimeId)
        {
            var consumption = await _dbContext.Consumptions.FindAsync(patientId, date);

            if (consumption == null)
            {
                return NotFound();
            }

            return consumption;
        }

        [HttpPost]
        public async Task<ActionResult<Consumption>> CreateConsumption(string patientId, DateTime date, int mealtimeId, int productBarcode)
        {
            var patientId_exists = await _dbContext.Patients.FindAsync(patientId);
            var mealtimeId_exists = await _dbContext.MealTimes.FindAsync(mealtimeId);
            var productBarcode_exists = await _dbContext.Products.FindAsync(productBarcode);
            if (patientId_exists == null)
            {
                return NotFound("Patient not found");
            }
            else if (mealtimeId_exists == null)
            {
                return NotFound("Mealtime not found");
            }
            else if (productBarcode_exists == null)
            {
                return NotFound("Product not found");
            }


            // Falta agrear los productos que se consumen
            var consumption = new Consumption
            {
                Patientid = patientId,
                Date = new DateOnly(date.Year, date.Month, date.Day),
                Mealtime = mealtimeId,
            };

            _dbContext.Consumptions.Add(consumption);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConsumption), new { patientId = consumption.Patientid, date = consumption.Date }, consumption);
        }

        [HttpPut("{patientId}/{date}")]
        public async Task<IActionResult> UpdateConsumption(string patientId, DateTime date, Consumption updatedConsumption)
        {
            var consumption = await _dbContext.Consumptions.FindAsync(patientId, date);

            if (consumption == null)
            {
                return NotFound();
            }

            consumption.Mealtime = updatedConsumption.Mealtime;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{patientId}/{date}")]
        public async Task<IActionResult> DeleteConsumption(string patientId, DateTime date)
        {
            var consumption = await _dbContext.Consumptions.FindAsync(patientId, date);

            if (consumption == null)
            {
                return NotFound();
            }

            _dbContext.Consumptions.Remove(consumption);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
