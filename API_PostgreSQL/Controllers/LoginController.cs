using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Postgre_API.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Postgre_API.Controllers
{
    [ApiController]
    [Route("api/Login")]
    public class LoginController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        public LoginController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private string EncryptPassword_MD5(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        [HttpPost]
        [Route("{email}/{password}")]
        public IActionResult LoginUser(string email, string password)
        {
            try
            {
                string encryptedPassword = EncryptPassword_MD5(password);
                dynamic user = _dbContext.Patients.FirstOrDefault(p => p.Email == email);
                string type = "patient";
                if (user == null)
                {
                    user = _dbContext.Nutritionists.FirstOrDefault(n => n.Email == email);
                    type = "nutritionist";
                    if (user == null)
                    {
                        user = _dbContext.Administrators.FirstOrDefault(n => n.Email == email);
                        type = "administrator";
                        if (user == null)
                        {
                            return NotFound(new { message = "User " + email + " is not registered" });
                        }
                    }
                }
                if (user.Password != encryptedPassword)
                {
                    return Unauthorized(new { message = "Password is incorrect" });
                }

                var result = new
                {
                    Usuario = user,
                    Tipo = type
                };

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
