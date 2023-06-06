using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Postgre_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Postgre_API.Controllers
{
    [Route("api/PatientNutritionistAssociation")]
    [ApiController]
    public class PatientNutritionistAssociationController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        public PatientNutritionistAssociationController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientNutritionistAssociation>>> GetPatientNutritionistAssociations()
        {
            try
            {
                return await _dbContext.PatientNutritionistAssociations.ToListAsync();
            }
            catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }
            
        }

        [HttpGet("{nutritionistId}/{patientId}")]
        public async Task<ActionResult<PatientNutritionistAssociation>> GetPatientNutritionistAssociation(string nutritionistId, string patientId)
        {
            try
            {
                var association = await _dbContext.PatientNutritionistAssociations.FindAsync(nutritionistId, patientId);

                if (association == null)
                {
                    return NotFound(new { message = "Association not found" });
                }

                return Ok(association);
            }
            catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatientNutritionistAssociation(string nutritionistId, string patientId)
        {
            try{
            var associationExists = await _dbContext.PatientNutritionistAssociations.FindAsync(nutritionistId, patientId);
            var patient = await _dbContext.Patients.FindAsync(patientId);
            var nutritionist = await _dbContext.Nutritionists.FindAsync(nutritionistId);

            if (associationExists != null)
            {
                return Content(new { message"Association already exists!"});
            }
            else if (patient == null)
            {
                return NotFound(new { message = "Patient not found" });
            }
            else if (nutritionist == null)
            {
                return NotFound(new { message = "Nutritionist not found" });
            }

            var association = new PatientNutritionistAssociation
            {
                Nutritionistid = nutritionistId,
                Patientid = patientId
            };

            _dbContext.PatientNutritionistAssociations.Add(association);
            await _dbContext.SaveChangesAsync();

            var options = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(association, options);

            return Ok(json);
        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}

        [HttpDelete("{nutritionistId}/{patientId}")]
        public async Task<IActionResult> DeletePatientNutritionistAssociation(string nutritionistId, string patientId)
        {
            try{
            var association = await _dbContext.PatientNutritionistAssociations.FindAsync(nutritionistId, patientId);

            if (association == null)
            {
                return NotFound(new { message = "Association not found" });
            }

            _dbContext.PatientNutritionistAssociations.Remove(association);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "ok" });
        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}
    }
}
