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
    public class RecipesController : ControllerBase
    {
        private readonly NutritecDbContext _context;

        public RecipesController(NutritecDbContext context)
        {
            _context = context;
        }

        // GET: api/Recipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            return await _context.Recipes.ToListAsync();
        }

        // GET: api/Recipes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return recipe;
        }

        // POST: api/Recipes
        [HttpPost]
        public async Task<ActionResult<Recipe>> CreateRecipe(string description, int productBarcode, int Productportion)
        {
            var productBarcode_exists = await _context.Products.FindAsync(productBarcode);
            if (productBarcode_exists == null)
            {
                return NotFound("Product not found");
            }
            var recipe = new Recipe
            {
                Description = description
            };
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            var recipeId = await _context.Recipes.FirstOrDefaultAsync(r => r.Description == description);
            var recipeProductAssociation = new RecipeProductAssociation
            {
                Recipeid = recipe.Id,
                Productbarcode = productBarcode,
                Productportion = Productportion,
            };
            _context.RecipeProductAssociations.Add(recipeProductAssociation);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetRecipe", new { id = recipe.Id }, recipe);
        }

        // PUT: api/Recipes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, string description, int productBarcode, int productPortion)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            var productBarcodeExists = await _context.Products.FindAsync(productBarcode);

            if (productBarcodeExists == null)
            {
                return NotFound("Product not found");
            }

            recipe.Description = description;

            var recipeProductAssociation = await _context.RecipeProductAssociations.FirstOrDefaultAsync(rpa => rpa.Recipeid == id);

            if (recipeProductAssociation != null)
            {
                recipeProductAssociation.Productbarcode = productBarcode;
                recipeProductAssociation.Productportion = productPortion;
            }
            else
            {
                recipeProductAssociation = new RecipeProductAssociation
                {
                    Recipeid = id,
                    Productbarcode = productBarcode,
                    Productportion = productPortion,
                };

                _context.RecipeProductAssociations.Add(recipeProductAssociation);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(recipeProductAssociation);
        }

        // DELETE: api/Recipes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            var recipeProductAssociation = await _context.RecipeProductAssociations.FirstOrDefaultAsync(rpa => rpa.Recipeid == id);

            if (recipeProductAssociation != null)
            {
                _context.RecipeProductAssociations.Remove(recipeProductAssociation);
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
