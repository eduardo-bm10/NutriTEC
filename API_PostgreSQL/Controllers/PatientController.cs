using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Postgre_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

        private string EncryptPasswordMD5(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            try{
            return await _dbContext.Patients.ToListAsync();
            }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}
        

        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(string id)
        {
            try{
                var patient = await _dbContext.Patients.FindAsync(id);

                if (patient == null)
                {
                    return NotFound(new { message = "Patient not found" });
                }

                return patient;

            }catch(Exception e){
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatient(string id, string firstname, string lastname1, string lastname2, string email, string password, int weight, double bmi, string address, DateTime birthdate, string country, double maxconsumption, double waist, double neck, double hips, double musclePercentage, double fatPercentage)
        {
            try
            {
            var patientExists = await _dbContext.Patients.FindAsync(id);

            if (patientExists != null)
            {
                return Content("Patient already exists!");
            }

            string encryptedPassword = EncryptPasswordMD5(password);

            var patient = new Patient
            {
                Id = id,
                Firstname = firstname,
                Lastname1 = lastname1,
                Lastname2 = lastname2,
                Email = email,
                Password = encryptedPassword,
                Weight = weight,
                Bmi = bmi,
                Address = address,
                Birthdate = new DateOnly(birthdate.Year, birthdate.Month, birthdate.Day),
                Country = country,
                Maxconsumption = maxconsumption
            };

            var measurements = new Measurement
            {
                Patientid = id,
                Date = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                Waist = waist,
                Neck = neck,
                Hips = hips,
                Musclepercentage = musclePercentage,
                Fatpercentage = fatPercentage
            };

            _dbContext.Patients.Add(patient);
            _dbContext.Measurements.Add(measurements);
            await _dbContext.SaveChangesAsync();

            var options = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(patient, options);

            return Ok(json);
            }
            catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }
            }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(string id, string firstname, string lastname1, string lastname2, string email, string password, int weight, double bmi, string address, DateTime birthdate, string country, double maxconsumption)
        {
            try
            {
            var patient = await _dbContext.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound(new { message = "Patient not found" });
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

            return Ok(new {message = "ok"});
        }
        catch (Exception e)
        {
            return BadRequest(new { message =  e.Message});
        }}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(string id)
        {
            try
            {
            var patient = await _dbContext.Patients.FindAsync(id);
            var measurementsForPatient = _dbContext.Measurements.Where(m => m.Patientid == id).ToList();

            if (patient == null)
            {
                return NotFound(new { message = "Patient not found" });
            }

            // Delete each measurement for the patient
            foreach (var measurement in measurementsForPatient)
            {
                _dbContext.Measurements.Remove(measurement);
            }

            _dbContext.Patients.Remove(patient);
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
