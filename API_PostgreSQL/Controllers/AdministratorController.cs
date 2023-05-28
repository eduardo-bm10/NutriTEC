using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgre_API.Models;

namespace Postgre_API.Controllers
{
    [Route("api/Administrator")]
    [ApiController]
    public class AdministratorsController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        public AdministratorsController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Administrator>>> GetAdministrators()
        {
            return await _dbContext.Administrators.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Administrator>> GetAdministrator(string id)
        {
            var administrator = await _dbContext.Administrators.FindAsync(id);

            if (administrator == null)
            {
                return NotFound();
            }

            return administrator;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Administrator>> CreateAdministrator(string id, string firstname, string lastname1, string lastname2, string email, string password)
        {
            var administrator = new Administrator
            {
                Id = id,
                Firstname = firstname,
                Lastname1 = lastname1,
                Lastname2 = lastname2,
                Email = email,
                Password = password
            };

            _dbContext.Administrators.Add(administrator);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAdministrator), new { id = administrator.Id }, administrator);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdministrator(string id, string firstname, string lastname1, string lastname2, string email, string password)
        {
            var administrator = await _dbContext.Administrators.FindAsync(id);

            if (administrator == null)
            {
                return NotFound();
            }

            administrator.Firstname = firstname;
            administrator.Lastname1 = lastname1;
            administrator.Lastname2 = lastname2;
            administrator.Email = email;
            administrator.Password = password;

            _dbContext.Entry(administrator).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdministrator(string id)
        {
            var administrator = await _dbContext.Administrators.FindAsync(id);

            if (administrator == null)
            {
                return NotFound();
            }

            _dbContext.Administrators.Remove(administrator);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
