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
            try
            {
                var measurements = await _dbContext.Measurements.ToListAsync();
                return Ok(measurements);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("{patientId}/{date}")]
        public async Task<ActionResult<Measurement>> GetMeasurement(int patientId, DateTime date)
        {
            try
            {
                var measurement = await _dbContext.Measurements.FindAsync(patientId, new DateOnly(date.Year, date.Month, date.Day));

                if (measurement == null)
                {
                    return NotFound(new { message = "Measurement not found" });
                }

                return Ok(measurement);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("{patientId}")]
        public async Task<ActionResult<Measurement>> CreateMeasurement(string patientId, double waist, double neck, double hips, double musclePercentage, double fatPercentage)
        {
            try
            {
                DateOnly dateOnly = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var measurement0 = _dbContext.Measurements.FirstOrDefault(m => m.Date == dateOnly && m.Patientid == patientId);
                var patientIdExists = await _dbContext.Patients.FindAsync(patientId);

                if (measurement0 != null)
                {
                    return Content("Measurement already exists!");
                }
                else if (patientIdExists == null)
                {
                    return NotFound(new { message = "Patient not found" });
                }

                var measurement = new Measurement
                {
                    Patientid = patientId,
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
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut("{patientId}/{date}")]
        public async Task<IActionResult> UpdateMeasurement(int patientId, DateTime date, double waist, double neck, double hips, double musclePercentage, double fatPercentage)
        {
            try
            {
                var measurement = await _dbContext.Measurements.FindAsync(patientId, new DateOnly(date.Year, date.Month, date.Day));

                if (measurement == null)
                {
                    return NotFound(new { message = "Measurement not found" });
                }

                var updatedMeasurement = new Measurement
                {
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
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpDelete("{patientId}/{date}")]
        public async Task<IActionResult> DeleteMeasurement(int id, DateTime date)
        {
            try
            {

                var measurement = await _dbContext.Measurements.FindAsync(id, new DateOnly(date.Year, date.Month, date.Day));

                if (measurement == null)
                {
                    return NotFound(new { message = "Measurement not found" });
                }

                _dbContext.Measurements.Remove(measurement);
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
