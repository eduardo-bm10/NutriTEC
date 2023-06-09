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

        [HttpGet("getOnlyIds")]
        public async Task<ActionResult<IEnumerable<Dictionary<string, object>>>> GetPlanOnlyId()
        {
            try{
                var plans = await _dbContext.Plans.ToListAsync();
                
                if (plans == null)
                {
                    return NotFound(new { message = "Plan not found" });
                }

                List<Dictionary<string,object>> allPlansIds = new List<Dictionary<string, object>>();
                foreach(var plan in plans){
                    Dictionary<string, object> planIds = new Dictionary<string, object>();
                    planIds["planid"] = plan.Id;
                    allPlansIds.Add(planIds);
                }

                var options = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                string json = JsonConvert.SerializeObject(allPlansIds, options);

                return Ok(json);
            }catch(Exception e){
                return BadRequest(new {message = e.Message});
            }
        }

        /// <summary>
        /// Retrieves all plans.
        /// </summary>
        /// <returns>A list of plans.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dictionary<string, object>>>> GetPlans()
        {
            try{

                var plans = await _dbContext.Plans.ToListAsync();
                // Obtener las PlanProductAssociation para cada plan
                var planMealtimeAssocs = await _dbContext.PlanMealtimeAssociations.ToListAsync();
                var mealtime1Products = new List<int>();
                var mealtime2Products = new List<int>();
                var mealtime3Products = new List<int>();
                var mealtime4Products = new List<int>();
                var mealtime5Products = new List<int>();

                // Obtener los productos para cada PlanProductAssociation
                foreach (var planMealtimeAssoc in planMealtimeAssocs)
                {
                    //Rellenar los productos de cada comida
                    var product = await _dbContext.Products.FindAsync(planMealtimeAssoc.Productbarcode);
                    switch (planMealtimeAssoc.Mealtimeid)
                    {
                        case 1:
                            mealtime1Products.Add(product.Barcode);
                            break;
                        case 2:
                            mealtime2Products.Add(product.Barcode);
                            break;
                        case 3:
                            mealtime3Products.Add(product.Barcode);
                            break;
                        case 4:
                            mealtime4Products.Add(product.Barcode);
                            break;
                        case 5:
                            mealtime5Products.Add(product.Barcode);
                            break;
                    }
                    
                }

                // Retornar los planes con sus productos
                List<Dictionary<string, object>> plansWithProducts = new List<Dictionary<string, object>>();
                foreach (var plan in plans)
                {
                    var planWithProducts = new Dictionary<string, object>();
                    planWithProducts["planid"] = plan.Id;
                    planWithProducts["description"] = plan.Description;
                    planWithProducts["mealtime1"] = mealtime1Products;
                    planWithProducts["mealtime2"]= mealtime2Products;
                    planWithProducts["mealtime3"]= mealtime3Products;
                    planWithProducts["mealtime4"]= mealtime4Products;
                    planWithProducts["mealtime5"]= mealtime5Products;
                    plansWithProducts.Add(planWithProducts);
                }

                var options = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                string json = JsonConvert.SerializeObject(plansWithProducts, options);

                return Ok(json);

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
        public async Task<ActionResult<Dictionary<string, object>>> GetPlan(int id)
        {
            try{
                var plan = await _dbContext.Plans.FindAsync(id);

                if (plan == null){
                    return NotFound(new {message = "Plan not found"});
                }

                // Obtener las PlanProductAssociation para el plan
                var planMealTimeAssocs = await _dbContext.PlanMealtimeAssociations.Where(p => p.Planid == id).ToListAsync();

                // Obtener los productos para cada PlanProductAssociation y almacenarlos en un DICCIONARIO para cada comida
                var mealtime1Products = new List<Product>();
                var mealtime2Products = new List<Product>();
                var mealtime3Products = new List<Product>();
                var mealtime4Products = new List<Product>();
                var mealtime5Products = new List<Product>();
                
                // Obtener los productos para cada PlanProductAssociation
                foreach (var planMealtimeAssoc in planMealTimeAssocs)
                {
                    //Rellenar los productos de cada comida
                    var product = await _dbContext.Products.FindAsync(planMealtimeAssoc.Productbarcode);
                    switch (planMealtimeAssoc.Mealtimeid)
                    {
                        case 1:
                            mealtime1Products.Add(product);
                            break;
                        case 2:
                            mealtime2Products.Add(product);
                            break;
                        case 3:
                            mealtime3Products.Add(product);
                            break;
                        case 4:
                            mealtime4Products.Add(product);
                            break;
                        case 5:
                            mealtime5Products.Add(product);
                            break;
                    }
                }

                Dictionary<string, object> planWithProducts = new Dictionary<string, object>();
                planWithProducts["planid"] = plan.Id;
                planWithProducts["description"] = plan.Description;
                planWithProducts["nutrionistid"] = plan.Nutritionistid;
                planWithProducts["mealtime1"] = mealtime1Products;
                planWithProducts["mealtime2"] = mealtime2Products;
                planWithProducts["mealtime3"] = mealtime3Products;
                planWithProducts["mealtime4"] = mealtime4Products;
                planWithProducts["mealtime5"] = mealtime5Products;
                planWithProducts["message"] = "ok";
                var options = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                string json = JsonConvert.SerializeObject(planWithProducts, options);

                return Ok(json);
                
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


                // Crear PlanMealtimeAssociation por cada productListArray individual
                // Recorriendo productsList1_array
                foreach (string product in productsList1_array)
                {
                    PlanMealtimeAssociation planMealtimeAssociation = new PlanMealtimeAssociation();

                    planMealtimeAssociation.Planid = planId_;
                    planMealtimeAssociation.Mealtimeid = 1;
                    planMealtimeAssociation.Productbarcode = Convert.ToInt32(product);

                    _dbContext.PlanMealtimeAssociations.Add(planMealtimeAssociation);
                    await _dbContext.SaveChangesAsync();
                }

                // Recorriendo productsList2_array
                foreach (string product in productsList2_array)
                {
                    PlanMealtimeAssociation planMealtimeAssociation = new PlanMealtimeAssociation();

                    planMealtimeAssociation.Planid = planId_;
                    planMealtimeAssociation.Mealtimeid = 2;
                    planMealtimeAssociation.Productbarcode = Convert.ToInt32(product);

                    _dbContext.PlanMealtimeAssociations.Add(planMealtimeAssociation);
                    await _dbContext.SaveChangesAsync();
                }

                // Recorriendo productsList3_array
                foreach (string product in productsList3_array)
                {
                    PlanMealtimeAssociation planMealtimeAssociation = new PlanMealtimeAssociation();

                    planMealtimeAssociation.Planid = planId_;
                    planMealtimeAssociation.Mealtimeid = 3;
                    planMealtimeAssociation.Productbarcode = Convert.ToInt32(product);

                    _dbContext.PlanMealtimeAssociations.Add(planMealtimeAssociation);
                    await _dbContext.SaveChangesAsync();
                }

                // Recorriendo productsList4_array
                foreach (string product in productsList4_array)
                {
                    PlanMealtimeAssociation planMealtimeAssociation = new PlanMealtimeAssociation();

                    planMealtimeAssociation.Planid = planId_;
                    planMealtimeAssociation.Mealtimeid = 4;
                    planMealtimeAssociation.Productbarcode = Convert.ToInt32(product);

                    _dbContext.PlanMealtimeAssociations.Add(planMealtimeAssociation);
                    await _dbContext.SaveChangesAsync();
                }

                // Recorriendo productsList5_array
                foreach (string product in productsList5_array)
                {
                    PlanMealtimeAssociation planMealtimeAssociation = new PlanMealtimeAssociation();

                    planMealtimeAssociation.Planid = planId_;
                    planMealtimeAssociation.Mealtimeid = 5;
                    planMealtimeAssociation.Productbarcode = Convert.ToInt32(product);

                    _dbContext.PlanMealtimeAssociations.Add(planMealtimeAssociation);
                    await _dbContext.SaveChangesAsync();
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
        public async Task<IActionResult> UpdatePlan(int id, string nutritionistId, string description)
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
                
                // Actualizar plan
                plan.Description = description;
                plan.Nutritionistid = nutritionistId;

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

                 // Eliminar PlanMealtimeAssociations de este plan
                var planMealtimeAssociations = await _dbContext.PlanMealtimeAssociations.Where(p => p.Planid == id).ToListAsync();
                if (planMealtimeAssociations == null)
                {
                    return NotFound(new { message = "PlanMealtimeAssociations not found" });
                }
                foreach (PlanMealtimeAssociation planMealtimeAssociation in planMealtimeAssociations)
                {
                    _dbContext.PlanMealtimeAssociations.Remove(planMealtimeAssociation);
                    await _dbContext.SaveChangesAsync();
                }

                // Eliminar PlanPatientAssociation de este plan
                var planPatientAssociations = await _dbContext.PlanPatientAssociations.Where(p => p.Planid == id).ToListAsync();
                if (planPatientAssociations == null)
                {
                    return NotFound(new { message = "PlanPatientAssociations not found" });
                }
                foreach (PlanPatientAssociation planPatientAssociation in planPatientAssociations)
                {
                    _dbContext.PlanPatientAssociations.Remove(planPatientAssociation);
                    await _dbContext.SaveChangesAsync();
                }
    
                // Borrar plan
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
