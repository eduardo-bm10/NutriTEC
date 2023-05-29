using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgre_API.Models;

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
            return await _dbContext.PatientNutritionistAssociations.ToListAsync();
        }

        [HttpGet("{nutritionistId}/{patientId}")]
        public async Task<ActionResult<PatientNutritionistAssociation>> GetPatientNutritionistAssociation(string nutritionistId, string patientId)
        {
            var association = await _dbContext.PatientNutritionistAssociations.FindAsync(nutritionistId, patientId);

            if (association == null)
            {
                return NotFound();
            }

            return association;
        }

        [HttpPost]
        public async Task<ActionResult<PatientNutritionistAssociation>> CreatePatientNutritionistAssociation(string nutritionistId, string patientId)
        {
            var association0 = await _dbContext.PatientNutritionistAssociations.FindAsync(nutritionistId, patientId);
            if (association0 != null)
            {
                return Content("Association already exists!");
            }

            var association = new PatientNutritionistAssociation
            {
                Nutritionistid = nutritionistId,
                Patientid = patientId
            };
            
            _dbContext.PatientNutritionistAssociations.Add(association);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatientNutritionistAssociation), new { nutritionistId = association.Nutritionistid, patientId = association.Patientid }, association);
        }

        [HttpDelete("{nutritionistId}/{patientId}")]
        public async Task<IActionResult> DeletePatientNutritionistAssociation(string nutritionistId, string patientId)
        {
            var association = await _dbContext.PatientNutritionistAssociations.FindAsync(nutritionistId, patientId);

            if (association == null)
            {
                return NotFound();
            }

            _dbContext.PatientNutritionistAssociations.Remove(association);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
