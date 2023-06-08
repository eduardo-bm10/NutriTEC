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
    /// <summary>
    /// Controller for handling user login.
    /// </summary>
    [ApiController]
    [Route("api/Login")]
    public class LoginController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
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

        /// <summary>
        /// Logs in a user with the provided email and password.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>An action result indicating the login status and user information.</returns>
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
