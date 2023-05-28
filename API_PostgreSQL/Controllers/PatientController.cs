using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgre_API.Models;

namespace Postgre_API.Controllers
{
    [Route("api/Patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        public PatientsController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return await _dbContext.Patients.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(string id)
        {
            var patient = await _dbContext.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> CreatePatient(Patient patient)
        {
            _dbContext.Patients.Add(patient);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(string id, Patient updatedPatient)
        {
            var patient = await _dbContext.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            patient.Firstname = updatedPatient.Firstname;
            patient.Lastname1 = updatedPatient.Lastname1;
            patient.Lastname2 = updatedPatient.Lastname2;
            patient.Email = updatedPatient.Email;
            patient.Password = updatedPatient.Password;
            patient.Weight = updatedPatient.Weight;
            patient.Bmi = updatedPatient.Bmi;
            patient.Address = updatedPatient.Address;
            patient.Birthdate = updatedPatient.Birthdate;
            patient.Country = updatedPatient.Country;
            patient.Maxconsumption = updatedPatient.Maxconsumption;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(string id)
        {
            var patient = await _dbContext.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            _dbContext.Patients.Remove(patient);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
