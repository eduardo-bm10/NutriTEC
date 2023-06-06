
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgre_API.Models;
using Newtonsoft.Json;

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

        [HttpGet("{patientId}")]
        public async Task<ActionResult<Measurement>> GetMeasurement(int patientId)
        {
            var measurement = await _dbContext.Measurements.FindAsync(patientId);

            if (measurement == null)
            {
                return NotFound();
            }

            return measurement;
        }

        [HttpPost("{patientId}")]
        public async Task<ActionResult<Measurement>> CreateMeasurement(string patiendId,double waist, double neck, double hips, double musclePercentage, double fatPercentage)
        {
            DateOnly dateOnly = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var measurement0 = _dbContext.Measurements.FirstOrDefault(m => m.Date == dateOnly && m.Patientid == patiendId);
            var patientId_exists = await _dbContext.Patients.FindAsync(patiendId);
            if (measurement0 != null)
            {
                return Content("Measurement already exists!");
            }else if(patientId_exists == null){
                return NotFound("Patient not found!");
            }

            var measurement = new Measurement{
                Patientid = patiendId,
                Date = dateOnly,
                Waist = waist,
                Neck = neck,
                Hips = hips,
                Musclepercentage = musclePercentage,
                Fatpercentage = fatPercentage
            };

            _dbContext.Measurements.Add(measurement);
            await _dbContext.SaveChangesAsync();

           var options = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(measurement, options);

            return Ok(json);
        }

        [HttpPut("{patientId}")]
        public async Task<IActionResult> UpdateMeasurement(int patientId, DateTime date, double waist, double neck, double hips, double musclePercentage, double fatPercentage)
        {
            var measurement = await _dbContext.Measurements.FindAsync(patientId, new DateOnly(date.Year, date.Month, date.Day));

            if (measurement == null)
            {
                return NotFound();
            }

            var updatedMeasurement = new Measurement{
                Waist = waist,
                Neck = neck,
                Hips = hips,
                Musclepercentage = musclePercentage,
                Fatpercentage = fatPercentage
            };

            measurement.Waist = updatedMeasurement.Waist;
            measurement.Neck = updatedMeasurement.Neck;
            measurement.Hips = updatedMeasurement.Hips;
            measurement.Musclepercentage = updatedMeasurement.Musclepercentage;
            measurement.Fatpercentage = updatedMeasurement.Fatpercentage;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{patientId}/{}")]
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
