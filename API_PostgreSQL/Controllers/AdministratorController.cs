using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Postgre_API.Models;

namespace Postgre_API.Controllers
{
    /// <summary>
    /// Controller for managing administrators.
    /// </summary>
    [Route("api/Administrator")]
    [ApiController]
    public class AdministratorsController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdministratorsController"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public AdministratorsController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private dynamic encryptPassword_MD5(string password)
        {
            string encryptedPassword = "";
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                encryptedPassword = sb.ToString();
                Console.WriteLine(sb.ToString()); // borrar luego
            }
            return encryptedPassword;
        }
        

        /// <summary>
        /// Retrieves all administrators.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Administrator>>> GetAdministrators()
        {
            try{
                return await _dbContext.Administrators.ToListAsync();

            }catch(Exception e){
                Console.WriteLine(e);
                return BadRequest(new { message = e.Message });
            } 
        }

        /// <summary>
        /// Retrieves a specific administrator by ID.
        /// </summary>
        /// <param name="id">The ID of the administrator to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("get/{id}")]
        public async Task<ActionResult<Administrator>> GetAdministrator(string id)
        {
            try
            {
                var administrator = await _dbContext.Administrators.FindAsync(id);

                if (administrator == null)
                {
                    return NotFound(new { message = "Administrator not found" });
                }

                return administrator;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Creates a new administrator.
        /// </summary>
        /// <param name="id">The ID of the administrator.</param>
        /// <param name="firstname">The firstname of the administrator.</param>
        /// <param name="lastname1">The first lastname of the administrator.</param>
        /// <param name="lastname2">The second lastname of the administrator.</param>
        /// <param name="email">The email of the administrator.</param>
        /// <param name="password">The password of the administrator.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("post/{id}/{firstname}/{lastname1}/{lastname2}/{email}/{password}")]
        public async Task<ActionResult<Administrator>> CreateAdministrator(string id, string firstname, string lastname1, string lastname2, string email, string password)
        {
            try
            {
                string thePassword = encryptPassword_MD5(password);
                var administrator = new Administrator
                {
                    Id = id,
                    Firstname = firstname,
                    Lastname1 = lastname1,
                    Lastname2 = lastname2,
                    Email = email,
                    Password = thePassword
                };

                _dbContext.Administrators.Add(administrator);
                await _dbContext.SaveChangesAsync();

                return Ok(administrator);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Updates an existing administrator.
        /// </summary>
        /// <param name="id">The ID of the administrator to update.</param>
        /// <param name="firstname">The new firstname of the administrator.</param>
        /// <param name="lastname1">The new first lastname of the administrator.</param>
        /// <param name="lastname2">The new second lastname of the administrator.</param>
        /// <param name="email">The new email of the administrator.</param>
        /// <param name="password">The new password of the administrator.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPut("put/")]
        public async Task<IActionResult> UpdateAdministrator(string id, string firstname, string lastname1, string lastname2, string email, string password)
        {
            try
            {
                var administrator = await _dbContext.Administrators.FindAsync(id);
                if (administrator == null)
                {
                    return NotFound(new { message = "Administrator not found" });
                }

                string thePassword = encryptPassword_MD5(password);
                administrator.Firstname = firstname;
                administrator.Lastname1 = lastname1;
                administrator.Lastname2 = lastname2;
                administrator.Email = email;
                administrator.Password = thePassword;

                _dbContext.Entry(administrator).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Deletes an administrator.
        /// </summary>
        /// <param name="id">The ID of the administrator to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAdministrator(string id)
        {
            try
            {
                var administrator = await _dbContext.Administrators.FindAsync(id);
                var allAdminProductAssoc = await _dbContext.AdminProductAssociations
                    .Where(a => a.Adminid == id)
                    .ToListAsync(); // Get all associations with this admin

                if (administrator == null)
                {
                    return NotFound(new { message = "Administrator not found" });
                }
                // If there are no associations with this admin, just delete the admin
                else if (allAdminProductAssoc != null && allAdminProductAssoc.Count == 0)
                {
                    _dbContext.Administrators.Remove(administrator);
                    await _dbContext.SaveChangesAsync();
                    return Ok(new { message = "ok" });
                }

                // Else Delete all associations with this admin and the admin
                _dbContext.AdminProductAssociations.RemoveRange(allAdminProductAssoc);
                await _dbContext.SaveChangesAsync();

                // Remove the administrator entity
                _dbContext.Administrators.Remove(administrator);
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
