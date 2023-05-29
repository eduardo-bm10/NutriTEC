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

        [HttpPost("{adminId}/{productBarcode}")]
        public async Task<ActionResult<AdminProductAssociation>> CreateAdminProductAssociation(string adminId, int productBarcode, bool status)
        {

             var product0 = await _dbContext.Products.FindAsync(productBarcode);
             var admin = await _dbContext.Administrators.FindAsync(adminId);

            if (product0 == null || admin == null)
            {
                return Content("Product or Admin does not exist!");
            }

            var _adminProductAssociation = new AdminProductAssociation();
            _adminProductAssociation.Adminid = adminId;
            _adminProductAssociation.Productbarcode = productBarcode;

            product0.Status = status;
            _dbContext.AdminProductAssociations.Add(_adminProductAssociation);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAdminProductAssociation), new { adminId = _adminProductAssociation.Adminid, productBarcode = _adminProductAssociation.Productbarcode }, _adminProductAssociation);
        }

        [HttpPut("{adminId}/{productBarcode}")]
        public async Task<IActionResult> UpdateAdminProductAssociation(string adminId, int productBarcode)
        {
            var adminProductAssociation = await _dbContext.AdminProductAssociations.FindAsync(adminId, productBarcode);

            if (adminProductAssociation == null)
            {
                return NotFound();
            }


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
