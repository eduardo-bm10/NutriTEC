
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgre_API.Models;

namespace Postgre_API.Controllers
{
    [Route("api/Measurements")]
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        public MeasurementsController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurements()
        {
            return await _dbContext.Measurements.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Measurement>> GetMeasurement(int id)
        {
            var measurement = await _dbContext.Measurements.FindAsync(id);

            if (measurement == null)
            {
                return NotFound();
            }

            return measurement;
        }

        [HttpPost]
        public async Task<ActionResult<Measurement>> CreateMeasurement(Measurement measurement)
        {
            _dbContext.Measurements.Add(measurement);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMeasurement), new { id = measurement.Patientid }, measurement);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeasurement(int id, Measurement updatedMeasurement)
        {
            var measurement = await _dbContext.Measurements.FindAsync(id);

            if (measurement == null)
            {
                return NotFound();
            }

            measurement.Patientid = updatedMeasurement.Patientid;
            measurement.Date = updatedMeasurement.Date;
            measurement.Waist = updatedMeasurement.Waist;
            measurement.Neck = updatedMeasurement.Neck;
            measurement.Hips = updatedMeasurement.Hips;
            measurement.Musclepercentage = updatedMeasurement.Musclepercentage;
            measurement.Fatpercentage = updatedMeasurement.Fatpercentage;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeasurement(int id)
        {
            var measurement = await _dbContext.Measurements.FindAsync(id);

            if (measurement == null)
            {
                return NotFound();
            }

            _dbContext.Measurements.Remove(measurement);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
