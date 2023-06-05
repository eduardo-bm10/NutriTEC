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
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        public LoginController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private dynamic encryptPassword_MD5(string password){
            string encryptedPassword = "";
            using (MD5 md5 = MD5.Create()) {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes) {
                    sb.Append(b.ToString("x2"));
                }
                encryptedPassword = sb.ToString();
                Console.WriteLine(sb.ToString()); // borrar luego
            }            
            return encryptedPassword;
        }

        [HttpPost]
        [Route("login/{email}/{password}")]
        public dynamic LoginUser(string email, string password){
            var thePassword = encryptPassword_MD5(password);
            dynamic user = _dbContext.Patients.FirstOrDefault(p => p.Email == email);
            string type = "patient";

            if(user == null){
                user = _dbContext.Nutritionists.FirstOrDefault(n => n.Email == email);
                type = "nutritionist";
                if(user == null){
                    user = _dbContext.Administrators.FirstOrDefault(n => n.Email == email);
                    type = "administrator";
                }
                if(user.Password != thePassword){
                    return Content("Wrong password");
                }
            }

            var returnObject = new
            {
                User = user,
                Type = type
            };

            return new JsonResult(returnObject);
        }
}
}