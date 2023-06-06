using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Postgre_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Postgre_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanPatientAssociationsController : ControllerBase
    {
        private readonly NutritecDbContext _context;

        public PlanPatientAssociationsController(NutritecDbContext context)
        {
            _context = context;
        }

        // GET: api/PlanPatientAssociations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanPatientAssociation>>> GetPlanPatientAssociations()
        {
            try{
            return await _context.PlanPatientAssociations.ToListAsync();
            }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}
        

        // GET: api/PlanPatientAssociations/5
        [HttpGet("{patientid}/{planid}")]
        public async Task<ActionResult<PlanPatientAssociation>> GetPlanPatientAssociation(string patientid, int planid)
        {
            try{
            var planPatientAssociation = await _context.PlanPatientAssociations.FindAsync(patientid, planid);

            if (planPatientAssociation == null)
            {
                return NotFound(new {message = "PlanPatientAssociation not found"});
            }

            return planPatientAssociation;
        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}

        // POST: api/PlanPatientAssociations
        [HttpPost]
        public async Task<ActionResult<PlanPatientAssociation>> CreatePlanPatientAssociation(string nutritionistId,int planId, string patientId, DateTime startdate, DateTime enddate)
        {
            try{            
            var plan_exists = await _context.Plans.FindAsync(planId);
            var patientId_exists = await _context.Patients.FindAsync(patientId);
            var nutritionist_exists = await _context.Nutritionists.FindAsync(nutritionistId);
            var PatientNutritionistAssociation_exists = await _context.PatientNutritionistAssociations.FindAsync(nutritionistId, patientId);
            var planPatientAssociation_exists = await _context.PlanPatientAssociations.FindAsync(patientId, planId);
            if(plan_exists == null){
                return NotFound("Plan not found!");
            }else if(patientId_exists == null){
                return NotFound("Patient not found!");
            }else if(nutritionist_exists == null){
                return NotFound("Nutritionist not found!");
            }else if(PatientNutritionistAssociation_exists == null){
                return NotFound("Patient not associated with Nutritionist!");
            }else if(planPatientAssociation_exists != null){
                return NotFound("Plan already associated with Patient!");
            }

            var planPatientAssociation = new PlanPatientAssociation
            {
                Patientid = patientId,
                Planid = planId,
                Startdate = new DateOnly(startdate.Year, startdate.Month, startdate.Day),
                Enddate = new DateOnly(enddate.Year, enddate.Month, enddate.Day),
            };
            _context.PlanPatientAssociations.Add(planPatientAssociation);
            await _context.SaveChangesAsync();

            var options = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(planPatientAssociation, options);
           return Ok(json);
        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}

        // PUT: api/PlanPatientAssociations/5
        [HttpPut("{patientid}/{planid}")]
        public async Task<IActionResult> UpdatePlanPatientAssociation(string patientid, int planid, PlanPatientAssociation planPatientAssociation)
        {
            try{
            if (patientid != planPatientAssociation.Patientid || planid != planPatientAssociation.Planid)
            {
                return BadRequest();
            }

            _context.Entry(planPatientAssociation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanPatientAssociationExists(patientid, planid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }}

        // DELETE: api/PlanPatientAssociations/5
        [HttpDelete("{patientid}/{planid}")]
        public async Task<IActionResult> DeletePlanPatientAssociation(string patientid, int planid)
        {
            try{
            var planPatientAssociation = await _context.PlanPatientAssociations.FindAsync(patientid, planid);
            if (planPatientAssociation == null)
            {
                return NotFound();
            }

            _context.PlanPatientAssociations.Remove(planPatientAssociation);
            await _context.SaveChangesAsync();

            return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }}
        

        private bool PlanPatientAssociationExists(string patientid, int planid)
        {
            return _context.PlanPatientAssociations.Any(e => e.Patientid == patientid && e.Planid == planid);
        }
    }
}
