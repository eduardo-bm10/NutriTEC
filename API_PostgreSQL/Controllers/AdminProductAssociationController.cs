
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgre_API.Models;

namespace Postgre_API.Controllers
{
    [Route("api/AdminProductAssociations")]
    [ApiController]
    public class AdminProductAssociationsController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        public AdminProductAssociationsController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminProductAssociation>>> GetAdminProductAssociations()
        {
            return await _dbContext.AdminProductAssociations.ToListAsync();
        }

        [HttpGet("{adminId}/{productBarcode}")]
        public async Task<ActionResult<AdminProductAssociation>> GetAdminProductAssociation(string adminId, int productBarcode)
        {
            var adminProductAssociation = await _dbContext.AdminProductAssociations.FindAsync(adminId, productBarcode);

            if (adminProductAssociation == null)
            {
                return NotFound();
            }

            return adminProductAssociation;
        }

        [HttpPost]
        public async Task<ActionResult<AdminProductAssociation>> CreateAdminProductAssociation(AdminProductAssociation adminProductAssociation)
        {
            _dbContext.AdminProductAssociations.Add(adminProductAssociation);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAdminProductAssociation), new { adminId = adminProductAssociation.Adminid, productBarcode = adminProductAssociation.Productbarcode }, adminProductAssociation);
        }

        [HttpPut("{adminId}/{productBarcode}")]
        public async Task<IActionResult> UpdateAdminProductAssociation(string adminId, int productBarcode, AdminProductAssociation updatedAdminProductAssociation)
        {
            var adminProductAssociation = await _dbContext.AdminProductAssociations.FindAsync(adminId, productBarcode);

            if (adminProductAssociation == null)
            {
                return NotFound();
            }

            adminProductAssociation.Filler = updatedAdminProductAssociation.Filler;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{adminId}/{productBarcode}")]
        public async Task<IActionResult> DeleteAdminProductAssociation(string adminId, int productBarcode)
        {
            var adminProductAssociation = await _dbContext.AdminProductAssociations.FindAsync(adminId, productBarcode);

            if (adminProductAssociation == null)
            {
                return NotFound();
            }

            _dbContext.AdminProductAssociations.Remove(adminProductAssociation);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
