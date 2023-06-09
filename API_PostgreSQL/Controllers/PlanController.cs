using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgre_API.Models;
using Newtonsoft.Json;

namespace Postgre_API.Controllers
{
    [Route("api/Plan")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        public PlansController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all plans.
        /// </summary>
        /// <returns>A list of plans.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plan>>> GetPlans()
        {
            try{
            return await _dbContext.Plans.ToListAsync();
            }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}

        /// <summary>
        /// Retrieves a specific plan by its ID.
        /// </summary>
        /// <param name="id">The ID of the plan.</param>
        /// <returns>The plan with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Plan>> GetPlan(int id)
        {
            try{
                var plan = await _dbContext.Plans.FindAsync(id);

                if (plan == null){
                    return NotFound(new {message = "Plan not found"});
                }

                return plan;
            }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }
        }

        /// <summary>
        /// Creates a new plan.
        /// </summary>
        /// <param name="description">The description of the plan.</param>
        /// <param name="nutritionistId">The ID of the nutritionist associated with the plan.</param>
        /// <param name="productsList1">The list of products for mealtime 1.</param>
        /// <param name="productsList2">The list of products for mealtime 2.</param>
        /// <param name="productsList3">The list of products for mealtime 3.</param>
        /// <param name="productsList4">The list of products for mealtime 4.</param>
        /// <param name="productsList5">The list of products for mealtime 5.</param>
        /// <returns>The created plan.</returns>
        [HttpPost("post/{description}")]
        public async Task<ActionResult<Plan>> CreatePlan(string description, string nutritionistId, string productsList1, string productsList2, string productsList3, string productsList4, string productsList5)
        {
            try{
                // Verificar que el nutritionista exista
                var nutritionistId_exists = await _dbContext.Nutritionists.FindAsync(nutritionistId);
                if (nutritionistId_exists == null)
                {
                    return NotFound(new { message = "Nutritionist not found" });
                }
                // Separar cada productsList por comas
                string[] productsList1_array = productsList1.Split(',');
                string[] productsList2_array = productsList2.Split(',');
                string[] productsList3_array = productsList3.Split(',');
                string[] productsList4_array = productsList4.Split(',');
                string[] productsList5_array = productsList5.Split(',');
                // Crear lista de listas de productos
                List<string[]>allLists = new List<string[]>();
                allLists.Add(productsList1_array);
                allLists.Add(productsList2_array);
                allLists.Add(productsList3_array);
                allLists.Add(productsList4_array);
                allLists.Add(productsList5_array);

                // Verificar que cada producto exista
                foreach (string[] productsList in allLists)
                {
                    foreach (string product in productsList)
                    {
                        var product_exists = await _dbContext.Products.FindAsync(Convert.ToInt32(product));
                        if (product_exists == null)
                        {
                            return NotFound(new { message = "Product not found" });
                        }
                    }
                }
                
                // Crear plan
                var plan = new Plan{
                    Description = description,
                    Nutritionistid = nutritionistId
                };
                _dbContext.Plans.Add(plan);
                await _dbContext.SaveChangesAsync();

                var plan0 = await _dbContext.Plans.FirstOrDefaultAsync(p => p.Description == description && p.Nutritionistid == nutritionistId);
                int planId_ = plan0.Id;


                // Crear PlanMealtimeAssociation por cada producto
                foreach (string[] productsList in allLists)
                {
                    foreach (string product in productsList)
                    {
                        PlanMealtimeAssociation planMealtimeAssociation = new PlanMealtimeAssociation();

                        planMealtimeAssociation.Planid = planId_;
                        planMealtimeAssociation.Mealtimeid = allLists.IndexOf(productsList) + 1;
                        planMealtimeAssociation.Productbarcode = Convert.ToInt32(product);
                        _dbContext.PlanMealtimeAssociations.Add(planMealtimeAssociation);
                        await _dbContext.SaveChangesAsync();

                    }
                }

                var options = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                string json = JsonConvert.SerializeObject(plan, options);
                return Ok(json);

            }catch (Exception e)
                {
                    return BadRequest(new {message = e.Message});
            }
        }

        
        /// <summary>
        /// Updates a specific plan.
        /// </summary>
        /// <param name="id">The ID of the plan to update.</param>
        /// <returns>An IActionResult indicating the result of the update operation.</returns>
        [HttpPut("put/{id}")]
        public async Task<IActionResult> UpdatePlan(int id, string nutritionistId, string description, string productsList1, string productsList2, string productsList3, string productsList4, string productsList5)
        {
            try{
                // Verificar que el plan exista
                var plan = await _dbContext.Plans.FindAsync(id);
                if (plan == null)
                {
                    return NotFound(new { message = "Plan not found" });
                }
                // Verificar que el nutritionista exista
                var nutritionistId_exists = await _dbContext.Nutritionists.FindAsync(nutritionistId);
                if (nutritionistId_exists == null)
                {
                    return NotFound(new { message = "Nutritionist not found" });
                }
                // Separar cada productsList por comas
                string[] productsList1_array = productsList1.Split(',');
                string[] productsList2_array = productsList2.Split(',');
                string[] productsList3_array = productsList3.Split(',');
                string[] productsList4_array = productsList4.Split(',');
                string[] productsList5_array = productsList5.Split(',');
                // Crear lista de listas de productos
                List<string[]>allLists = new List<string[]>();
                allLists.Add(productsList1_array);
                allLists.Add(productsList2_array);
                allLists.Add(productsList3_array);
                allLists.Add(productsList4_array);
                allLists.Add(productsList5_array);

                // Verificar que cada producto exista
                foreach (string[] productsList in allLists)
                {
                    foreach (string product in productsList)
                    {
                        var product_exists = await _dbContext.Products.FindAsync(Convert.ToInt32(product));
                        if (product_exists == null)
                        {
                            return NotFound(new { message = "Product not found" });
                        }
                    }
                }

                // Actualizar plan
                plan.Description = description;
                plan.Nutritionistid = nutritionistId;

                var plan0 = await _dbContext.Plans.FirstOrDefaultAsync(p => p.Description == description && p.Nutritionistid == nutritionistId);
                int planId_ = plan0.Id;

                // Actualizar cada PlanMealtimeAssociation
                foreach (string[] productsList in allLists)
                {
                    foreach (string product in productsList)
                    {
                        var planMealtimeAssociation = await _dbContext.PlanMealtimeAssociations.FindAsync(plan.Id, allLists.IndexOf(productsList) + 1, Convert.ToInt32(product));
                        planMealtimeAssociation.Planid = planId_;
                        planMealtimeAssociation.Mealtimeid = allLists.IndexOf(productsList) + 1;
                        planMealtimeAssociation.Productbarcode = Convert.ToInt32(product);
                    }
                }

                _dbContext.Entry(plan).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Deletes a specific plan.
        /// </summary>
        /// <param name="id">The ID of the plan to delete.</param>
        /// <returns>An IActionResult indicating the result of the delete operation.</returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePlan(int id)
        {
            try{
            var plan = await _dbContext.Plans.FindAsync(id);

            if (plan == null)
            {
                return NotFound(new { message = "Plan not found" });
            }

            _dbContext.Plans.Remove(plan);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        private bool PlanExists(int id)
        {
            return _dbContext.Plans.Any(e => e.Id == id);
        }
    }
}
