using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Postgre_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ActionResult<Nutritionist>> CreateNutritionist(Nutritionist nutritionist)
        {
            _context.Nutritionists.Add(nutritionist);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNutritionist", new { id = nutritionist.Id }, nutritionist);
        }

        // PUT: api/Nutritionists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNutritionist(string id, Nutritionist nutritionist)
        {
            if (id != nutritionist.Id)
            {
                return BadRequest();
            }

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
