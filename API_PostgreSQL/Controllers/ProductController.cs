using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Postgre_API.Models;

namespace Postgre_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        public ProductsController(NutritecDbContext context)
        {
            _dbContext = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _dbContext.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{barcode}")]
        public async Task<ActionResult<Product>> GetProduct(int barcode)
        {
            var product = await _dbContext.Products.FindAsync(barcode);

            if (product == null)
            {
                return NotFound();
            }

            product.Barcode = barcode;


            return NoContent();
        }

        // GET: api/Products/Tomato
        [HttpGet("description/{description}")]
        public async Task<ActionResult<Product>> GetProductByDescription(string description)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Description == description);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }


        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(int barcode, string description, double iron, double sodium, double energy, double fat, double calcium, double carbohydrate, double protein)
        {
            var product0 = await _dbContext.Products.FindAsync(barcode);

            if (product0 != null)
            {
                return Content("Product already exists!");
            }
            var product = new Product{
                Barcode = barcode,
                Description = description,
                Iron = iron,
                Sodium = sodium,
                Energy = energy,
                Fat = fat,
                Calcium = calcium,
                Carbohydrate = carbohydrate,
                Protein = protein,
                Status = false
                };

           _dbContext.Products.Add(product); 
           await _dbContext.SaveChangesAsync();

           return CreatedAtAction(nameof(GetProduct), new { barcode = product.Barcode }, product);
        }

        // PUT: api/Products/5
        [HttpPut("{barcode}")]
        public async Task<IActionResult> UpdateProduct(int barcode, string description, double iron, double sodium, double energy, double fat, double calcium, double carbohydrate, double protein, bool status)
        {
            var product = await _dbContext.Products.FindAsync(barcode);

            if (product == null)
            {
                return NotFound();
            }

            product.Barcode = barcode;
            product.Description = description;
            product.Iron = iron;
            product.Sodium = sodium;
            product.Energy = energy;
            product.Fat = fat;
            product.Calcium = calcium;
            product.Carbohydrate = carbohydrate;
            product.Protein = protein;
            product.Status = status;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Products/5
        [HttpDelete("{barcode}")]
        public async Task<IActionResult> DeleteProduct(int barcode)
        {
            var product = await _dbContext.Products.FindAsync(barcode);
            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int barcode)
        {
            return _dbContext.Products.Any(e => e.Barcode == barcode);
        }
    }
}
