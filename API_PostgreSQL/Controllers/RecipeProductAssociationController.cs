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
    public class RecipeProductAssociationsController : ControllerBase
    {
        private readonly NutritecDbContext _context;

        public RecipeProductAssociationsController(NutritecDbContext context)
        {
            _context = context;
        }

        // GET: api/RecipeProductAssociations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeProductAssociation>>> GetRecipeProductAssociations()
        {
            return await _context.RecipeProductAssociations.ToListAsync();
        }

        // GET: api/RecipeProductAssociations/5
        [HttpGet("{recipeid}/{productbarcode}")]
        public async Task<ActionResult<RecipeProductAssociation>> GetRecipeProductAssociation(int recipeid, int productbarcode)
        {
            var recipeProductAssociation = await _context.RecipeProductAssociations.FindAsync(recipeid, productbarcode);

            if (recipeProductAssociation == null)
            {
                return NotFound(new { message = "RecipeProductAssociation not found" });
            }

            return recipeProductAssociation;
        }

        // POST: api/RecipeProductAssociations
        [HttpPost]
        public async Task<ActionResult<RecipeProductAssociation>> CreateRecipeProductAssociation(RecipeProductAssociation recipeProductAssociation)
        {
            _context.RecipeProductAssociations.Add(recipeProductAssociation);
            await _context.SaveChangesAsync();

            return Ok(new { message = "ok" });
        }

        // PUT: api/RecipeProductAssociations/5
        [HttpPut("{recipeid}/{productbarcode}")]
        public async Task<IActionResult> UpdateRecipeProductAssociation(int recipeid, int productbarcode, RecipeProductAssociation recipeProductAssociation)
        {
            try{
            if (recipeid != recipeProductAssociation.Recipeid || productbarcode != recipeProductAssociation.Productbarcode)
            {
                return BadRequest(new { message = "RecipeProductAssociation not found" });
            }

            _context.Entry(recipeProductAssociation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeProductAssociationExists(recipeid, productbarcode))
                {
                    return NotFound(new { message = "RecipeProductAssociation not found" });
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

        // DELETE: api/RecipeProductAssociations/5
        [HttpDelete("{recipeid}/{productbarcode}")]
        public async Task<IActionResult> DeleteRecipeProductAssociation(int recipeid, int productbarcode)
        {
            try{
            var recipeProductAssociation = await _context.RecipeProductAssociations.FindAsync(recipeid, productbarcode);
            if (recipeProductAssociation == null)
            {
                return NotFound(new {message = "RecipeProductAssociation not found"});
            }

            _context.RecipeProductAssociations.Remove(recipeProductAssociation);
            await _context.SaveChangesAsync();

            return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }}

        private bool RecipeProductAssociationExists(int recipeid, int productbarcode)
        {
            return _context.RecipeProductAssociations.Any(e => e.Recipeid == recipeid && e.Productbarcode == productbarcode);
        }
    }
}
