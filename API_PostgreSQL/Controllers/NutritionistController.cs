using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    /// Controller for managing nutritionists.
    /// </summary>
    [Route("api/Nutritionists")]
    [ApiController]
    public class NutritionistsController : ControllerBase
    {
        private readonly NutritecDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="NutritionistsController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public NutritionistsController(NutritecDbContext context)
        {
            _context = context;
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
        /// Retrieves all nutritionists.
        /// </summary>
        /// <returns>A list of nutritionists.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nutritionist>>> GetNutritionists()
        {
            try
            {
                return await _context.Nutritionists.ToListAsync();
            }
            catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }
        }

        /// <summary>
        /// Retrieves a specific nutritionist by ID.
        /// </summary>
        /// <param name="id">The ID of the nutritionist.</param>
        /// <returns>The requested nutritionist.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Nutritionist>> GetNutritionist(string id)
        {
            try
            {
                var nutritionist = await _context.Nutritionists.FindAsync(id);

                if (nutritionist == null)
                {
                    return NotFound(new { message = "Nutritionist not found" });
                }

                return Ok(nutritionist);
            }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }
        }

        /// <summary>
        /// Creates a new nutritionist.
        /// </summary>
        /// <param name="id">The ID of the nutritionist.</param>
        /// <param name="nutritionistcode">The nutritionist code.</param>
        /// <param name="firstname">The first name of the nutritionist.</param>
        /// <param name="lastname1">The first last name of the nutritionist.</param>
        /// <param name="lastname2">The second last name of the nutritionist.</param>
        /// <param name="email">The email address of the nutritionist.</param>
        /// <param name="password">The password of the nutritionist.</param>
        /// <param name="weight">The weight of the nutritionist.</param>
        /// <param name="bmi">The BMI (Body Mass Index) of the nutritionist.</param>
        /// <param name="cardNumber">The card number of the nutritionist.</param>
        /// <param name="address">The address of the nutritionist.</param>
        /// <param name="photo">The photo of the nutritionist.</param>
        /// <param name="paymentid">The payment ID of the nutritionist.</param>
        /// <returns>The created nutritionist.</returns>
        [HttpPost]
        public async Task<ActionResult<Nutritionist>> CreateNutritionist(string id, string nutritionistcode, string firstname, string lastname1, string lastname2, string email, string password, int weight, double bmi, string cardNumber, string address, string photo, int paymentid = 0)
        {
            try
            {
                var nutritionistExists = await _context.Nutritionists.FindAsync(id);

                if (nutritionistExists != null)
                {
                    return BadRequest(new {message = "The nutritionist already exists!"});
                }

                string encryptedPassword = EncryptPasswordMD5(password);

                var nutritionist = new Nutritionist
                {
                    Id = id,
                    Nutritionistcode = nutritionistcode,
                    Firstname = firstname,
                    Lastname1 = lastname1,
                    Lastname2 = lastname2,
                    Email = email,
                    Password = encryptedPassword,
                    Weight = weight,
                    Bmi = bmi,
                    CardNumber = cardNumber,
                    Address = address,
                    Photo = photo,
                    Paymentid = paymentid
                };

                _context.Nutritionists.Add(nutritionist);
                await _context.SaveChangesAsync();

                return Ok(nutritionist);
            }
            catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }
        }

        /// <summary>
        /// Updates an existing nutritionist.
        /// </summary>
        /// <param name="id">The ID of the nutritionist.</param>
        /// <param name="nutritionistcode">The nutritionist code.</param>
        /// <param name="firstname">The updated first name of the nutritionist.</param>
        /// <param name="lastname1">The updated first last name of the nutritionist.</param>
        /// <param name="lastname2">The updated second last name of the nutritionist.</param>
        /// <param name="email">The updated email address of the nutritionist.</param>
        /// <param name="password">The updated password of the nutritionist.</param>
        /// <param name="weight">The updated weight of the nutritionist.</param>
        /// <param name="bmi">The updated BMI (Body Mass Index) of the nutritionist.</param>
        /// <param name="cardNumber">The updated card number of the nutritionist.</param>
        /// <param name="address">The updated address of the nutritionist.</param>
        /// <param name="photo">The updated photo of the nutritionist.</param>
        /// <param name="paymentid">The updated payment ID of the nutritionist.</param>
        /// <returns>The status of the update operation.</returns>
        [HttpPut("put/{id}")]
        public async Task<IActionResult> UpdateNutritionist(string id, string nutritionistcode, string firstname, string lastname1, string lastname2, string email, string password, int weight, double bmi, string cardNumber, string address, string photo, int paymentid = 0)
        {
            try
            {
                var nutritionistExists = await _context.Nutritionists.FindAsync(id);

                if (nutritionistExists == null)
                {
                    return BadRequest(new {message = "The nutritionist does not exist!"});
                }

                string encryptedPassword = EncryptPasswordMD5(password);

                var nutritionist = new Nutritionist
                {
                    Id = id,
                    Nutritionistcode = nutritionistcode,
                    Firstname = firstname,
                    Lastname1 = lastname1,
                    Lastname2 = lastname2,
                    Email = email,
                    Password = encryptedPassword,
                    Weight = weight,
                    Bmi = bmi,
                    CardNumber = cardNumber,
                    Address = address,
                    Photo = photo,
                    Paymentid = paymentid
                };

                _context.Entry(nutritionist).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NutritionistExists(id))
                    {
                        return NotFound(new { message = "Nutritionist not found" });
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok(new { message = "Nutritionist updated" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message =  e.Message});
            }
        }

        /// <summary>
        /// Deletes a nutritionist.
        /// </summary>
        /// <param name="id">The ID of the nutritionist to delete.</param>
        /// <returns>The status of the delete operation.</returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteNutritionist(string id)
        {
            try
            {
                var nutritionist = await _context.Nutritionists.FindAsync(id);

                if (nutritionist == null)
                {
                    return NotFound(new { message = "Nutritionist not found" });
                }

                _context.Nutritionists.Remove(nutritionist);
                await _context.SaveChangesAsync();

                return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        private bool NutritionistExists(string id)
        {
            return _context.Nutritionists.Any(e => e.Id == id);
        }
    }
}
