using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Postgre_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace Postgre_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NutritionistsController : ControllerBase
    {
        private readonly NutritecDbContext _context;

        public NutritionistsController(NutritecDbContext context)
        {
            _context = context;
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
                return NotFound();
            }

            return nutritionist;
        }

        // POST: api/Nutritionists
        [HttpPost]
        public async Task<ActionResult<Nutritionist>> CreateNutritionist(string id, string nutritionistcode, string firstname, string lastname1, string lastname2, string email, string password, int weight, double bmi,  int cardNumber,string address, byte[] photo, int paymentid = 0)
        {
            var nutritionist_exists = await _context.Nutritionists.FindAsync(id);

            if (nutritionist_exists != null)
            {
                return Content("The nutritionist already exists!");
            }
            string thePassword = encryptPassword_MD5(password);
            var nutritionist = new Nutritionist{
                Id = id,
                Nutritionistcode = nutritionistcode,
                Firstname = firstname,
                Lastname1 = lastname1,
                Lastname2 = lastname2,
                Email = email,
                Password = password,
                Weight = weight,
                Bmi = bmi, 
                CardNumber =cardNumber,
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
        public async Task<IActionResult> UpdateNutritionist(string id, string nutritionistcode, string firstname, string lastname1, string lastname2, string email, string password, int weight, double bmi, int cardNumber,string address, byte[] photo, int paymentid = 0)
        {
            var nutritionist_exists = await _context.Nutritionists.FindAsync(id);

            if (nutritionist_exists == null)
            {
                return Content("The nutritionist does not exists!");
            }
            string thePassword = encryptPassword_MD5(password);
            var nutritionist = new Nutritionist{
                Id = id,
                Nutritionistcode = nutritionistcode,
                Firstname = firstname,
                Lastname1 = lastname1,
                Lastname2 = lastname2,
                Email = email,
                Password = password,
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
                    return NotFound();
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
                return NotFound();
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
