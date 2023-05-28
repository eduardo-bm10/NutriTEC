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
    public class ProductsController : ControllerBase
    {
        private readonly NutritecDbContext _context;

        public ProductsController(NutritecDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{barcode}")]
        public async Task<ActionResult<Product>> GetProduct(int barcode)
        {
            var product = await _context.Products.FindAsync(barcode);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { barcode = product.Barcode }, product);
        }

        // PUT: api/Products/5
        [HttpPut("{barcode}")]
        public async Task<IActionResult> UpdateProduct(int barcode, Product product)
        {
            if (barcode != product.Barcode)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(barcode))
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

        // DELETE: api/Products/5
        [HttpDelete("{barcode}")]
        public async Task<IActionResult> DeleteProduct(int barcode)
        {
            var product = await _context.Products.FindAsync(barcode);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int barcode)
        {
            return _context.Products.Any(e => e.Barcode == barcode);
        }
    }
}
