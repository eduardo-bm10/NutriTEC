using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Postgre_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Postgre_API.Controllers
{
    /// <summary>
    /// Represents a controller for managing recipes.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly NutritecDbContext _context;


        /// <summary>
        /// Initializes a new instance of the <see cref="RecipesController"/> class.
        /// </summary>
        /// <param name="context">The NutritecDbContext instance.</param>
        public RecipesController(NutritecDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all recipes.
        /// </summary>
        /// <returns>A list of recipes.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            try{
            return await _context.Recipes.ToListAsync();
            }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}
        
        /// <summary>
        /// Retrieves a recipe by ID.
        /// </summary>
        /// <param name="id">The ID of the recipe.</param>
        /// <returns>The recipe with the specified ID.</returns>
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

        /// <summary>
        /// Creates a new recipe.
        /// </summary>
        /// <param name="description">The description of the recipe.</param>
        /// <param name="BarcodeProducts">The barcode products associated with the recipe.</param>
        /// <param name="PortionProducts">The portion products associated with the recipe.</param>
        /// <returns>The created recipe.</returns>
        [HttpPost("post/{description}/{BarcodeProducts}/{PortionProducts}")]
        public async Task<ActionResult<Recipe>> CreateRecipe(string description, string  BarcodeProducts, string PortionProducts)
        {
            try{
            var recipeList = new List<Recipe>();
            var valoresProducts = BarcodeProducts.Split(',');
            var valoresPortions = PortionProducts.Split(',');

            int length = Math.Min(valoresProducts.Length, valoresPortions.Length);
            var recipe = new Recipe
            {
                Description = description
            };
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            for (int i = 0; i < length; i++)
            {
                int productBarcode = int.Parse(valoresProducts[i]);
                int Product_portion =  int.Parse(valoresPortions[i]);
   
                 var productBarcode_exists = await _context.Products.FindAsync(productBarcode);
                 
                if (productBarcode_exists == null)
                {
                    return NotFound(new {message = "Product not found"});
                }
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

        /// <summary>
        /// Updates an existing recipe.
        /// </summary>
        /// <param name="id">The ID of the recipe.</param>
        /// <param name="description">The description of the recipe.</param>
        /// <param name="BarcodeProducts">The barcode products associated with the recipe.</param>
        /// <param name="PortionProducts">The portion products associated with the recipe.</param>
        /// <returns>A response indicating the update status.</returns>
        [HttpPut("put/{id}/{description}/{BarcodeProducts}/{PortionProducts}")]
        public async Task<IActionResult> UpdateRecipe(int id, string description, string  BarcodeProducts, string PortionProducts)
        {
         try{
            var recipe0 = await _context.Recipes.FindAsync(id);

            if (recipe0 == null)
            {
                return NotFound(new {message = "Recipe not found"});
            }

            recipe0.Description = description;
            _context.Entry(recipe0).State = EntityState.Modified;
            await _context.SaveChangesAsync();

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

                var recipeId = await _context.Recipes.FirstOrDefaultAsync(r => r.Description == description);
                var recipeProductAssociation = await _context.RecipeProductAssociations.FirstOrDefaultAsync(rpa => rpa.Recipeid == id && rpa.Productbarcode == productBarcode);
                recipeProductAssociation.Productportion = Product_portion;

                _context.Entry(recipeProductAssociation).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            var options = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return Ok(new { message = "ok" });

        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}

        /// <summary>
        /// Deletes a recipe by ID.
        /// </summary>
        /// <param name="id">The ID of the recipe.</param>
        /// <returns>A response indicating the deletion status.</returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            try{
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound(new {message = "Recipe not found"});
            }

            var recipeProductAssociation = await _context.RecipeProductAssociations
                    .Where(rpa => rpa.Recipeid == id)
                    .ToListAsync();

            // Cuando hay asocs
            if (recipeProductAssociation != null)
            {
                _context.RecipeProductAssociations.RemoveRange(recipeProductAssociation);
                await _context.SaveChangesAsync();
            }

            // Cuando no hay asocs
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return Ok(new { message = "ok" });
            
        }catch (Exception e)
        {
            return BadRequest(new {message = e.Message});
        }
        }
        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
