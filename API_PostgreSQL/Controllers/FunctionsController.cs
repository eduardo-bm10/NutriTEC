using Npgsql;
using Microsoft.AspNetCore.Mvc;
using Postgre_API.Functions;
using Postgre_API.Models;

namespace Postgre_API.Controllers {
    /// <summary>
    /// Controller for calling database functions.
    /// </summary>
    [Route("api/Functions")]
    [ApiController]
    public class FunctionsControllers : ControllerBase {

        /// <summary>
        /// Retrieves the payment report for a given payment ID.
        /// </summary>
        /// <param name="paymentId">The ID of the payment.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("getPaymentReport/{paymentId}")]
        public async Task<IEnumerable<PaymentReport>> getPaymentReport(int paymentId) {
            string connectionString = "Server=server-nutritec.postgres.database.azure.com;Database=nutritec-db;Port=5432;User Id=jimena;Password=Nutri_TEC;Ssl Mode=VerifyFull;";
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            string commandQuery= $"SELECT Email, FullName, TotalPayment, Discount, FinalPayment FROM calculate_payment(@payment)";
            await using (NpgsqlCommand command = new NpgsqlCommand(commandQuery, con)) {
                command.Parameters.AddWithValue("@payment", paymentId);
                await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync()) {
                    var result = new List<PaymentReport>();
                    while (await reader.ReadAsync()) {
                        string email = reader["Email"] as string;
                        string fullname = reader["FullName"] as string;
                        int? totalpayment = reader["TotalPayment"] as int?;
                        string discount = reader["Discount"] as string;
                        float? finalpayment = reader["FinalPayment"] as float?;
                        PaymentReport report = new PaymentReport {
                            Email = email,
                            FullName = fullname,
                            TotalPayment = totalpayment,
                            Discount = discount,
                            FinalPayment = finalpayment
                        };
                        result.Add(report);
                    }
                    con.Close();
                    return result;
                }
            }
            con.Close();
            return null;
        }

        /// <summary>
        /// Retrieves the advance report for a given patient ID and date range.
        /// </summary>
        /// <param name="patientId">The ID of the patient.</param>
        /// <param name="startDate">The start date of the report.</param>
        /// <param name="finalDate">The end date of the report.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("getAdvanceReport/{patientId}/{startDate}/{finalDate}")]
        public async Task<IEnumerable<CustomersAdvanceReport>> getAdvanceReport(string patientId, DateTime startDate, DateTime finalDate) {
            string connectionString = "Server=server-nutritec.postgres.database.azure.com;Database=nutritec-db;Port=5432;User Id=jimena;Password=Nutri_TEC;Ssl Mode=VerifyFull;";
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            string commandQuery= $"SELECT Patient, ReDate, Waist, Neck, Hips, MusclePercentage, FatPercentage FROM customers_advance_report(@patient, @date1, @date2)";
            await using (NpgsqlCommand command = new NpgsqlCommand(commandQuery, con)) {
                command.Parameters.AddWithValue("@patient", patientId);
                command.Parameters.AddWithValue("@date1", new DateOnly(startDate.Year, startDate.Month, startDate.Day));
                command.Parameters.AddWithValue("@date2", new DateOnly(finalDate.Year, finalDate.Month, finalDate.Day));
                await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync()) {
                    var result = new List<CustomersAdvanceReport>();
                    while (await reader.ReadAsync()) {
                        string patient = reader["Patient"] as string;
                        DateTime? date = reader["ReDate"] as DateTime?;
                        float? waist = reader["Waist"] as float?;
                        float? neck = reader["Neck"] as float?;
                        float? hips = reader["Hips"] as float?;
                        float? muscle = reader["MusclePercentage"] as float?;
                        float? fat = reader["FatPercentage"] as float?;
                        CustomersAdvanceReport report = new CustomersAdvanceReport {
                            PatientId = patient,
                            Date = date.ToString(),
                            Waist = waist,
                            Neck = neck,
                            Hips = hips,
                            MusclePercentage = muscle,
                            FatPercentage = fat
                        };
                        result.Add(report);
                    }
                    con.Close();
                    return result;
                }
            }
            con.Close();
            return null;
        }

        /// <summary>
        /// Creates a new recipe.
        /// </summary>
        /// <param name="description">The description of the recipe.</param>
        /// <param name="products">The products used in the recipe (comma-separated).</param>
        /// <param name="portions">The portions of each product used in the recipe (comma-separated).</param>
        [HttpPost("createRecipe/{description}/{products}/{portions}")]
        public async Task<ActionResult<Recipe>> createRecipe(string description, string products, string portions) {
            var productList = Array.ConvertAll(products.Split(","), s => int.Parse(s));
            var portionList = Array.ConvertAll(portions.Split(","), s => int.Parse(s));
            string conString = "Server=server-nutritec.postgres.database.azure.com;Database=nutritec-db;Port=5432;User Id=jimena;Password=Nutri_TEC;Ssl Mode=VerifyFull;";
            NpgsqlConnection con = new NpgsqlConnection(conString);
            con.Open();
            string query = $"CALL create_recipe(@desc, @products, @portions)";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(query, con)) {
                cmd.Parameters.AddWithValue("@desc", description);
                cmd.Parameters.AddWithValue("@products", productList);
                cmd.Parameters.AddWithValue("@portions", portionList);
                await cmd.ExecuteNonQueryAsync();
                con.Close();
            }
            return Ok(new {message = "ok"});
        }

        /// <summary>
        /// Retrieves the list of vitamins for a given product barcode.
        /// </summary>
        /// <param name="barcode">The barcode of the product.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("getVitamins/{barcode}")]
        public async Task<IEnumerable<Vitamin>> getVitamins(int barcode) {
            string conString = "Server=server-nutritec.postgres.database.azure.com;Database=nutritec-db;Port=5432;User Id=jimena;Password=Nutri_TEC;Ssl Mode=VerifyFull;";
            NpgsqlConnection con = new NpgsqlConnection(conString);
            con.Open();
            string cmdQuery = $"SELECT Vitamin FROM vitamins_per_product(@code)";
            await using (NpgsqlCommand cmd = new NpgsqlCommand(cmdQuery, con)) {
                cmd.Parameters.AddWithValue("@code", barcode);
                await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync()) {
                    var result = new List<Vitamin>();
                    while (await reader.ReadAsync()) {
                        string vitamin = reader["Vitamin"] as string;
                        Vitamin v = new Vitamin {
                            ProductBarcode = barcode,
                            Vitamin1 = vitamin
                        };
                        result.Add(v);
                    }
                    con.Close();
                    return result;
                }
            }
            con.Close();
            return null;
        }

        /// <summary>
        /// Retrieves the calories per mealtime on a plan.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("getCaloriesPerPlan")]
        public async Task<IEnumerable<ViewCaloriesPerMealtimeOnPlan>> getCaloriesPerPlan() {
            string connectionString = "Server=server-nutritec.postgres.database.azure.com;Database=nutritec-db;Port=5432;User Id=jimena;Password=Nutri_TEC;Ssl Mode=VerifyFull;";
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            string query = $"SELECT PlanId, Mealtime, TotalCalories FROM CaloriesPerMealTimeOnPlan";
            await using (NpgsqlCommand command = new NpgsqlCommand(query, con)) {
                await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync()) {
                    var result = new List<ViewCaloriesPerMealtimeOnPlan>();
                    while (await reader.ReadAsync()) {
                        int? planid = reader["PlanId"] as int?;
                        string mealtime = reader["Mealtime"] as string;
                        Double? totalcalories = reader["TotalCalories"] as Double?;
                        ViewCaloriesPerMealtimeOnPlan total = new ViewCaloriesPerMealtimeOnPlan {
                            PlanId = planid,
                            Mealtime = mealtime,
                            TotalCalories = totalcalories,
                        };
                        result.Add(total);
                    }
                    con.Close();
                    return result;
                }
            }
            con.Close();
            return null;
        }

        /// <summary>
        /// Retrieves the total calories for each recipe.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("getRecipeCalories")]
        public async Task<IEnumerable<ViewTotalRecipeCalories>> getRecipeCalories() {
            string connectionString = "Server=server-nutritec.postgres.database.azure.com;Database=nutritec-db;Port=5432;User Id=jimena;Password=Nutri_TEC;Ssl Mode=VerifyFull;";
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            string query = $"SELECT RecipeID, RecipeDescription, Product, ProductCalories, ProductPortion, TotalProductCalories, TotalRecipeCalories FROM TotalRecipeCalories";
            await using (NpgsqlCommand command = new NpgsqlCommand(query, con)) {
                await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync()) {
                    var result = new List<ViewTotalRecipeCalories>();
                    while (await reader.ReadAsync()) {
                        int? recipeid = reader["RecipeID"] as int?;
                        string recipedescription = reader["RecipeDescription"] as string;
                        string product = reader["Product"] as string;
                        Double? productcalories = reader["ProductCalories"] as Double?;
                        int? productportion = reader["ProductPortion"] as int?;
                        Double? totalproductcalories = reader["TotalProductCalories"] as Double?;
                        Double? totalrecipecalories = reader["TotalRecipeCalories"] as Double?;
                        ViewTotalRecipeCalories total = new ViewTotalRecipeCalories {
                            RecipeId = recipeid,
                            RecipeDescription = recipedescription,
                            ProductDescription = product,
                            ProductCalories = productcalories,
                            ProductPortion = productportion,
                            TotalProductCalories = totalproductcalories,
                            TotalRecipeCalories = totalrecipecalories
                        };
                        result.Add(total);
                    }
                    con.Close();
                    return result;
                }
            }
            con.Close();
            return null;
        }

        /// <summary>
        /// Retrieves the list of non-associated clients.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("getNotAssociatedClients")]
        public async Task<IEnumerable<ViewNonAssociatedClients>> getNotAssociatedClients() {
            string connectionString = "Server=server-nutritec.postgres.database.azure.com;Database=nutritec-db;Port=5432;User Id=jimena;Password=Nutri_TEC;Ssl Mode=VerifyFull;";
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            string query = $"SELECT PatientSSN, PatientName FROM NonAssociatedClients";
            await using (NpgsqlCommand command = new NpgsqlCommand(query, con)) {
                await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync()) {
                    var result = new List<ViewNonAssociatedClients>();
                    while (await reader.ReadAsync()) {
                        string ssn = reader["PatientSSN"] as string;
                        string name = reader["PatientName"] as string;
                        ViewNonAssociatedClients total = new ViewNonAssociatedClients {
                            PatientSSN = ssn,
                            PatientName = name
                        };
                        result.Add(total);
                    }
                    con.Close();
                    return result;
                }
            }
            con.Close();
            return null;
        }
    }
}