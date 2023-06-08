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
    /// <summary>
    /// Controller for managing patients.
    /// </summary>
    [Route("api/Patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientsController"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
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

        /// <summary>
        /// Retrieves all patients.
        /// </summary>
        /// <returns>A list of patients.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            try{
            return await _dbContext.Patients.ToListAsync();
            }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}
    
        /// <summary>
        /// Retrieves a specific patient by ID.
        /// </summary>
        /// <param name="id">The ID of the patient.</param>
        /// <returns>The requested patient.</returns>
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

        /// <summary>
        /// Creates a new patient.
        /// </summary>
        /// <param name="id">The ID of the patient.</param>
        /// <param name="firstname">The first name of the patient.</param>
        /// <param name="lastname1">The first last name of the patient.</param>
        /// <param name="lastname2">The second last name of the patient.</param>
        /// <param name="email">The email address of the patient.</param>
        /// <param name="password">The password of the patient.</param>
        /// <param name="weight">The weight of the patient.</param>
        /// <param name="bmi">The BMI (Body Mass Index) of the patient.</param>
        /// <param name="address">The address of the patient.</param>
        /// <param name="birthdate">The birthdate of the patient.</param>
        /// <param name="country">The country of the patient.</param>
        /// <param name="maxconsumption">The maximum consumption of the patient.</param>
        /// <param name="waist">The waist measurement of the patient.</param>
        /// <param name="neck">The neck measurement of the patient.</param>
        /// <param name="hips">The hips measurement of the patient.</param>
        /// <param name="musclePercentage">The muscle percentage of the patient.</param>
        /// <param name="fatPercentage">The fat percentage of the patient.</param>
        /// <returns>The created patient.</returns>
        [HttpPost("post/{id}/{firstname}/{lastname1}/{lastname2}/{email}/{password}/{weight}/{bmi}/{address}/{birthdate}/{country}/{maxconsumption}/{waist}/{neck}/{hips}/{musclePercentage}/{fatPercentage}")]
        public async Task<IActionResult> CreatePatient(string id, string firstname, string lastname1, string lastname2, string email, string password, int weight, double bmi, string address, DateTime birthdate, string country, double maxconsumption, double waist, double neck, double hips, double musclePercentage, double fatPercentage)
        {
            try
            {
            var patientExists = await _dbContext.Patients.FindAsync(id);

            if (patientExists != null)
            {
                return BadRequest("Patient already exists!");
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

        /// <summary>
        /// Updates an existing patient.
        /// </summary>
        /// <param name="id">The ID of the patient to update.</param>
        /// <param name="firstname">The updated first name of the patient.</param>
        /// <param name="lastname1">The updated first last name of the patient.</param>
        /// <param name="lastname2">The updated second last name of the patient.</param>
        /// <param name="email">The updated email address of the patient.</param>
        /// <param name="password">The updated password of the patient.</param>
        /// <param name="weight">The updated weight of the patient.</param>
        /// <param name="bmi">The updated BMI (Body Mass Index) of the patient.</param>
        /// <param name="address">The updated address of the patient.</param>
        /// <param name="birthdate">The updated birthdate of the patient.</param>
        /// <param name="country">The updated country of the patient.</param>
        /// <param name="maxconsumption">The updated maximum consumption of the patient.</param>
        /// <returns>The status of the update operation.</returns>
        [HttpPut("put/{id}")]
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

        /// <summary>
        /// Deletes a patient.
        /// </summary>
        /// <param name="id">The ID of the patient to delete.</param>
        /// <returns>The status of the delete operation.</returns>
        [HttpDelete("delete/{id}")]
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
