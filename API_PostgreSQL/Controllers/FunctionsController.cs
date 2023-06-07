using Npgsql;
using Microsoft.AspNetCore.Mvc;
using Postgre_API.Functions;

namespace Postgre_API.Controllers {
    [Route("api/Functions")]
    [ApiController]
    public class FunctionsControllers : ControllerBase {

        [HttpGet("getPaymentReport/{paymentId}")]
        public async Task<PaymentReport> getPaymentReport(int paymentId) {
            string connectionString = "Server=server-nutritec.postgres.database.azure.com;Database=nutritec-db;Port=5432;User Id=jimena;Password=Nutri_TEC;Ssl Mode=VerifyFull;";
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            string commandQuery= $"SELECT Email, FullName, TotalPayment, Discount, FinalPayment FROM calculate_payment(@payment)";
            await using (NpgsqlCommand command = new NpgsqlCommand(commandQuery, con)) {
                command.Parameters.AddWithValue("@payment", paymentId);
                await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync()) {
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
                        con.Close();
                        return report;
                    }
                }
            }
            con.Close();
            return null;
        }

        [HttpGet("getAdvanceReport/{patientId}/{startDate}/{finalDate}")]
        public async Task<CustomersAdvanceReport> getAdvanceReport(string patientId, DateTime startDate, DateTime finalDate) {
            string connectionString = "Server=server-nutritec.postgres.database.azure.com;Database=nutritec-db;Port=5432;User Id=jimena;Password=Nutri_TEC;Ssl Mode=VerifyFull;";
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            con.Open();
            string commandQuery= $"SELECT Patient, ReDate, Waist, Neck, Hips, MusclePercentage, FatPercentage FROM customers_advance_report(@patient, @date1, @date2)";
            await using (NpgsqlCommand command = new NpgsqlCommand(commandQuery, con)) {
                command.Parameters.AddWithValue("@patient", patientId);
                command.Parameters.AddWithValue("@date1", new DateOnly(startDate.Year, startDate.Month, startDate.Day));
                command.Parameters.AddWithValue("@date2", new DateOnly(finalDate.Year, finalDate.Month, finalDate.Day));
                await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync()) {
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
                        con.Close();
                        return report;
                    }
                }
            }
            con.Close();
            return null;
        }
    }
}