using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Postgre_API.Models;
using Newtonsoft.Json;

namespace Postgre_API.Controllers
{
    [Route("api/Product")]
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
            try{
            return await _dbContext.Products.ToListAsync();
            }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}
        

        [HttpGet("getByBarcode/{barcode}")]
        public async Task<ActionResult<Product>> GetProduct(int barcode)
        {
            try{            
            var product = await _dbContext.Products.FindAsync(barcode);

            if (product == null)
            {
                return NotFound(new {message = "Product not found"});
            }

            product.Barcode = barcode;


            var options = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(product, options);
           return Ok(json);
           return Ok(json);
        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }
        }

        [HttpGet("getByDescription/{description}")]
        public async Task<ActionResult<Product>> GetProductByDescription(string description)
        {
            try{
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Description == description);

            if (product == null)
            {
                return NotFound(new {message = "Product not found"});
            }

            var options = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(product, options);
           return Ok(json);
        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}


        [HttpPost("post/{barcode}")]
        public async Task<ActionResult<Product>> CreateProduct(int barcode, string description, double iron, double sodium, double energy, double fat, double calcium, double carbohydrate, double protein, string vitamins)
        {
            try{
            var product0 = await _dbContext.Products.FindAsync(barcode);

            if (product0 != null)
            {
                return BadRequest(new {message ="Product already exists!"});
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
                
            var vitaminas_todas = vitamins.Split(",");

            foreach (var vitaminaActual in vitaminas_todas)
            {
                var vitamina = new Vitamin{
                    ProductBarcode = barcode,
                    Vitamin1 = vitaminaActual
                };
            
                _dbContext.Vitamins.Add(vitamina);
            }
           _dbContext.Products.Add(product); 
           await _dbContext.SaveChangesAsync();

            var options = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(product, options);
           return Ok(json);
        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}
        
        [HttpGet("getByStatus/{status}")]
        public async Task<IActionResult> GetProductByStatus(bool status)
        {
            try{
            // Obtener el producto con status
            var specificProducts = await _dbContext.Products
                    .Where(a => a.Status == status)
                    .ToListAsync(); // Get all associations with this admin
            
            if (specificProducts.Count == 0){
                return BadRequest(new {message = "No products with this status"});
            }

            return Ok(specificProducts);
        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }
        }

        [HttpPut("put/{barcode}/{description}/{iron}/{sodium}/{energy}/{fat}/{calcium}/{carbohydrate}/{protein}/{vitamins}")]
        public async Task<IActionResult> UpdateProduct(int barcode, string description, double iron, double sodium, double energy, double fat, double calcium, double carbohydrate, double protein, string vitamins)
        {
            try{
            var product = await _dbContext.Products.FindAsync(barcode);

            if (product == null)
            {
                return NotFound(new {message = "Product not found"});
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


            var vitaminas_todas = vitamins.Split(",");//el parametro separado
            var vitList = await _dbContext.Vitamins.Where(v => v.ProductBarcode == barcode).ToListAsync();//todas las vitaminas del producto
            //actualizar descripcion de las vitaminas vitList
            foreach (var vitaminaActual in vitList)
            {
                vitaminaActual.Vitamin1 = vitaminas_todas[vitList.IndexOf(vitaminaActual)];
            }

            await _dbContext.SaveChangesAsync();

            var options = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string json = JsonConvert.SerializeObject(product, options);
           return Ok(json);
        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}

        

        // Pendiente involucra muchas tablas
        [HttpDelete("delete/{barcode}")]
        public async Task<IActionResult> DeleteProduct(int barcode)
        {
            try
            {
                // Recetas, consumo, vitaminas, AdminProductAssociation, Product
                var product = await _dbContext.Products.FindAsync(barcode);
                var recipes = await _dbContext.RecipeProductAssociations.Where(r => r.Productbarcode == barcode).ToListAsync();
                var allAdminProductAssoc = await _dbContext.AdminProductAssociations.Where(a => a.Productbarcode == barcode).ToListAsync();
                var consumptions = await _dbContext.Consumptions.Where(c => c.Productbarcode == barcode).ToListAsync();
                var vitamins = await _dbContext.Vitamins.Where(v => v.ProductBarcode == barcode).ToListAsync();

                if (product == null)
                {
                    return NotFound(new { message = "Product not found" });
                }

                //borrar todas las recetas que contengan el producto
                foreach (var recipe in recipes)
                {
                    _dbContext.RecipeProductAssociations.Remove(recipe);
                }

                //borrar todas las asociaciones de admin-producto que contengan el producto
                foreach (var adminProductAssoc in allAdminProductAssoc)
                {
                    _dbContext.AdminProductAssociations.Remove(adminProductAssoc);
                }

                //borrar todos los consumos que contengan el producto
                foreach (var consumption in consumptions)
                {
                    _dbContext.Consumptions.Remove(consumption);
                }

                //borrar todas las vitaminas que contengan el producto
                foreach (var vitamin in vitamins)
                {
                    _dbContext.Vitamins.Remove(vitamin);
                }
                
                _dbContext.Products.Remove(product);//elimina el producto
                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(new { message = e.Message });
            }
        }
    }
}