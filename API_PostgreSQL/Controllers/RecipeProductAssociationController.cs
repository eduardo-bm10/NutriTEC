using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Postgre_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        // GET: api/RecipeProductAssocs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeProductAssociation>>> GetRecipeProductAssocs()
        {
            return await _context.RecipeProductAssociations.ToListAsync();
        }

        [HttpGet("{recipeid}/{productbarcode}")]
        public async Task<ActionResult<RecipeProductAssociation>> GetRecipeProductAssoc(int recipeid, int productbarcode)
        {
            var recipeProductAssoc = await _context.RecipeProductAssociations.FindAsync(recipeid, productbarcode);

            if (recipeProductAssoc == null)
            {
                return NotFound(new { message = "RecipeProductAssociation not found" });
            }

            return recipeProductAssoc;
        }

        // POST: api/RecipeProductAssocs
        [HttpPost]
        public async Task<ActionResult<RecipeProductAssociation>> CreateRecipeProductAssoc(RecipeProductAssociation recipeProductAssoc)
        {
            _context.RecipeProductAssociations.Add(recipeProductAssoc);
            await _context.SaveChangesAsync();

            return Ok(new { message = "ok" });
        }

        [HttpGet("getProductsAndPortionsFromRecipe/{recipeid}")]
        public async Task<ActionResult<IEnumerable<Dictionary<string, object>>>> GetProductsAndPortionsFromRecipe(int recipeid)
        {
            var recipeProductAssoc = await _context.RecipeProductAssociations.Where(x => x.Recipeid == recipeid).ToListAsync();

            if (recipeProductAssoc == null)
            {
                return NotFound(new { message = "RecipeProductAssociation not found" });
            }

            // Create a list to hold the JSON objects
            List<Dictionary<string, object>> productsNameAndPortions = new List<Dictionary<string, object>>();

            foreach (var item in recipeProductAssoc)
            {
                var product = await _context.Products.FindAsync(item.Productbarcode);
                
                // Create a dictionary for each object
                Dictionary<string, object> jsonObject = new Dictionary<string, object>();
                jsonObject["productName"] = product.Description;
                jsonObject["productportion"] = item.Productportion;

                // Add the dictionary to the list
                productsNameAndPortions.Add(jsonObject);
            }

            Console.WriteLine(productsNameAndPortions);

            // Return the JSON result
            return productsNameAndPortions;
        }

        // PUT: api/RecipeProductAssocs/5
        [HttpPut("{recipeid}/{productbarcode}")]
        public async Task<IActionResult> UpdateRecipeProductAssoc(int recipeid, int productbarcode, RecipeProductAssociation recipeProductAssoc)
        {
            try{
            if (recipeid != recipeProductAssoc.Recipeid || productbarcode != recipeProductAssoc.Productbarcode)
            {
                return BadRequest(new { message = "RecipeProductAssociation not found" });
            }

            _context.Entry(recipeProductAssoc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeProductAssocExists(recipeid, productbarcode))
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

        // DELETE: api/RecipeProductAssocs/5
        [HttpDelete("{recipeid}/{productbarcode}")]
        public async Task<IActionResult> DeleteRecipeProductAssoc(int recipeid, int productbarcode)
        {
            try{
            var recipeProductAssoc = await _context.RecipeProductAssociations.FindAsync(recipeid, productbarcode);
            if (recipeProductAssoc == null)
            {
                return NotFound(new {message = "RecipeProductAssociation not found"});
            }

            _context.RecipeProductAssociations.Remove(recipeProductAssoc);
            await _context.SaveChangesAsync();

            return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }}

        private bool RecipeProductAssocExists(int recipeid, int productbarcode)
        {
            return _context.RecipeProductAssociations.Any(e => e.Recipeid == recipeid && e.Productbarcode == productbarcode);
        }
    }
}
