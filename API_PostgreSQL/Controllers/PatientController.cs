using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgre_API.Models;
using System.Security.Cryptography;
using System.Text;

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

        private dynamic encryptPassword_MD5(string password){
            string encryptedPassword = "";
            using (MD5 md5 = MD5.Create()) {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes) {
                    sb.Append(b.ToString("x2"));
                }
                encryptedPassword = sb.ToString();
                Console.WriteLine(sb.ToString()); // borrar luego
            }            
            return encryptedPassword;
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
        public async Task<ActionResult<Patient>> CreatePatient(string id,string firstname,string lastname1,string lastname2,string email,string password,int weight,double bmi,string address,DateTime birthdate,string country,double maxconsumption, double waist, double neck, double hips, double musclePercentage, double fatPercentage)
        {
            var patient0 = await _dbContext.Patients.FindAsync(id);

            if (patient0 != null)
            {
                return Content("Patient already exists!");
            }
            string thePassword = encryptPassword_MD5(password);
            var patient = new Patient
            {
                Id = id,
                Firstname = firstname,
                Lastname1 = lastname1,
                Lastname2 = lastname2,
                Email = email,
                Password = thePassword,
                Weight = weight,
                Bmi = bmi,
                Address = address,
                Birthdate = new DateOnly(birthdate.Year, birthdate.Month, birthdate.Day),
                Country = country,
                Maxconsumption = maxconsumption
            };

            var measurements = new Measurement
            {
                Date = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                Waist = waist,
                Neck = neck,
                Hips = hips,
                Musclepercentage = musclePercentage,
                Fatpercentage = fatPercentage
            };

            _dbContext.Measurements.Add(measurements);
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
            patient.Birthdate = new DateOnly(birthdate.Year, birthdate.Month, birthdate.Day);
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
