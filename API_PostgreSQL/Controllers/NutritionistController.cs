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
    [Route("api/Nutritionists")]
    [ApiController]
    public class NutritionistsController : ControllerBase
    {
        private readonly NutritecDbContext _context;

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

        // GET: api/Nutritionists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nutritionist>>> GetNutritionists()
        {
            return await _context.Nutritionists.ToListAsync();
        }

        // GET: api/Nutritionists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Nutritionist>> GetNutritionist(string id)
        {
            var nutritionist = await _context.Nutritionists.FindAsync(id);

            if (nutritionist == null)
            {
                return NotFound(new { message = "Nutritionist not found" });
            }

            return nutritionist;
        }

        // POST: api/Nutritionists
        [HttpPost]
        public async Task<ActionResult<Nutritionist>> CreateNutritionist(string id, string nutritionistcode, string firstname, string lastname1, string lastname2, string email, string password, int weight, double bmi, string cardNumber, string address, string photo, int paymentid = 0)
        {
            var nutritionistExists = await _context.Nutritionists.FindAsync(id);

            if (nutritionistExists != null)
            {
                return Content("The nutritionist already exists!");
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

            return CreatedAtAction("GetNutritionist", new { id = nutritionist.Id }, nutritionist);
        }

        // PUT: api/Nutritionists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNutritionist(string id, string nutritionistcode, string firstname, string lastname1, string lastname2, string email, string password, int weight, double bmi, string cardNumber, string address, string photo, int paymentid = 0)
        {
            var nutritionistExists = await _context.Nutritionists.FindAsync(id);

            if (nutritionistExists == null)
            {
                return Content("The nutritionist does not exist!");
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

            return NoContent();
        }

        // DELETE: api/Nutritionists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNutritionist(string id)
        {
            var nutritionist = await _context.Nutritionists.FindAsync(id);

            if (nutritionist == null)
            {
                return NotFound(new { message = "Nutritionist not found" });
            }

            _context.Nutritionists.Remove(nutritionist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NutritionistExists(string id)
        {
            return _context.Nutritionists.Any(e => e.Id == id);
        }
    }
}
