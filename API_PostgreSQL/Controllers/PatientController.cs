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
        public async Task<ActionResult<Patient>> CreatePatient(string id,string firstname,string lastname1,string lastname2,string email,string password,int weight,double bmi,string address,DateTime birthdate,string country,double maxconsumption)
        {
            var patient = new Patient
            {
                Id = id,
                Firstname = firstname,
                Lastname1 = lastname1,
                Lastname2 = lastname2,
                Email = email,
                Password = password,
                Weight = weight,
                Bmi = bmi,
                Address = address,
                Birthdate = birthdate,
                Country = country,
                Maxconsumption = maxconsumption
            };

            _dbContext.Patients.Add(patient);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(string id, string firstname, string lastname1, string lastname2, string email, string password, int weight, double bmi, string address, DateTime birthdate, string country, double maxconsumption)
        {
            var patient = await _dbContext.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            patient.Firstname = firstname;
            patient.Lastname1 = lastname1;
            patient.Lastname2 = lastname2;
            patient.Email = email;
            patient.Password = password;
            patient.Weight = weight;
            patient.Bmi = bmi;
            patient.Address = address;
            patient.Birthdate = new DateTime(birthdate.Year, birthdate.Month, birthdate.Day, 0, 0, 0);;
            patient.Country = country;
            patient.Maxconsumption = maxconsumption;

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
