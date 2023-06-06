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
            try{
            return await _context.Recipes.ToListAsync();
            }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}
        

        // GET: api/Recipes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(int id)
        {
            try{
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound(new {message = "Recipe not found"});
            }

            return recipe;
        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}

        // POST: api/Recipes
        [HttpPost]
        public async Task<ActionResult<Recipe>> CreateRecipe(string description, string  BarcodeProducts, string PortionProducts)
        {
            try{
            var recipeList = new List<Recipe>();
            var valoresProducts = BarcodeProducts.Split(',');
            var valoresPortions = PortionProducts.Split(',');

            int length = Math.Min(valoresProducts.Length, valoresPortions.Length);

            for (int i = 0; i < length; i++)
            {
                int productBarcode = int.Parse(valoresProducts[i]);
                int Product_portion =  int.Parse(valoresPortions[i]);
   
                 var productBarcode_exists = await _context.Products.FindAsync(productBarcode);
                if (productBarcode_exists == null)
                {
                    return NotFound(new {message = "Product not found"});
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
                    Productportion = Product_portion
                };
                _context.RecipeProductAssociations.Add(recipeProductAssociation);
                await _context.SaveChangesAsync();
                recipeList.Add(recipe);
            }
            var options = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(recipeList, options);

            return Ok(json);

        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}

        // PUT: api/Recipes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, string description, int productBarcode, int productPortion)
        {
            try{
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound(new {message = "Recipe not found"});
            }

            var productBarcodeExists = await _context.Products.FindAsync(productBarcode);

            if (productBarcodeExists == null)
            {
                return NotFound(new {message = "Product not found"});
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
                    return NotFound(new {message = "Recipe not found"});
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "ok" });

        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}

        // DELETE: api/Recipes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            try{
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound(new {message = "Recipe not found"});
            }

            var recipeProductAssociation = await _context.RecipeProductAssociations.FirstOrDefaultAsync(rpa => rpa.Recipeid == id);

            if (recipeProductAssociation != null)
            {
                _context.RecipeProductAssociations.Remove(recipeProductAssociation);
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return Ok(new { message = "ok" });
            
        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}
        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
