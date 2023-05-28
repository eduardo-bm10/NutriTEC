using Microsoft.AspNetCore.Mvc;
using Postgre_API.Models;
using System;
using System.Linq;

namespace Postgre_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NutritionistController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        public NutritionistController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("login")]
    public dynamic SignUpNutritionist(string id, string nutritionistcode, string firstname, string lastname1, string lastname2, string email, string password, int weight, double bmi, string address, byte[]? photo = null, int paymentid = 0)
    {
        try
        {
            return new {message = "ok"};
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    }
}