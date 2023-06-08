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
    /// Represents a controller for managing recipe-product associations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeProductAssociationsController : ControllerBase
    {
        private readonly NutritecDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecipeProductAssociationsController"/> class.
        /// </summary>
        /// <param name="context">The NutritecDbContext instance.</param>
        public RecipeProductAssociationsController(NutritecDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all recipe-product associations.
        /// </summary>
        /// <returns>A list of recipe-product associations.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeProductAssociation>>> GetRecipeProductAssocs()
        {
            return await _context.RecipeProductAssociations.ToListAsync();
        }

        /// <summary>
        /// Retrieves a recipe-product association by recipe ID and product barcode.
        /// </summary>
        /// <param name="recipeid">The ID of the recipe.</param>
        /// <param name="productbarcode">The barcode of the product.</param>
        /// <returns>The recipe-product association with the specified recipe ID and product barcode.</returns>
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

        /// <summary>
        /// Creates a new recipe-product association.
        /// </summary>
        /// <param name="recipeProductAssoc">The recipe-product association to create.</param>
        /// <returns>The created recipe-product association.</returns>
        [HttpPost]
        public async Task<ActionResult<RecipeProductAssociation>> CreateRecipeProductAssoc(RecipeProductAssociation recipeProductAssoc)
        {
            _context.RecipeProductAssociations.Add(recipeProductAssoc);
            await _context.SaveChangesAsync();

            return Ok(new { message = "ok" });
        }

        /// <summary>
        /// Retrieves products and portions associated with a recipe.
        /// </summary>
        /// <param name="recipeid">The ID of the recipe.</param>
        /// <returns>A list of dictionaries containing product names and portions.</returns>
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

        /// <summary>
        /// Updates an existing recipe-product association.
        /// </summary>
        /// <param name="recipeid">The ID of the recipe.</param>
        /// <param name="productbarcode">The barcode of the product.</param>
        /// <param name="recipeProductAssoc">The updated recipe-product association.</param>
        /// <returns>A response indicating the update status.</returns>
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


        /// <summary>
        /// Deletes a recipe-product association by recipe ID and product barcode.
        /// </summary>
        /// <param name="recipeid">The ID of the recipe.</param>
        /// <param name="productbarcode">The barcode of the product.</param>
        /// <returns>A response indicating the deletion status.</returns>
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
