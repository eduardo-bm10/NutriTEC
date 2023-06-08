using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgre_API.Models;
using Newtonsoft.Json;

namespace Postgre_API.Controllers
{
    /// <summary>
    /// Controller for managing admin-product associations.
    /// </summary>
    [Route("api/AdminProductAssociations")]
    [ApiController]
    public class AdminProductAssociationsController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminProductAssociationsController"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public AdminProductAssociationsController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all admin-product associations.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminProductAssociation>>> GetAdminProductAssociations()
        {
            try
            {
                return await _dbContext.AdminProductAssociations.ToListAsync();
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Retrieves a specific admin-product association by admin ID and product barcode.
        /// </summary>
        /// <param name="adminId">The ID of the admin.</param>
        /// <param name="productBarcode">The barcode of the product.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpGet("{adminId}/{productBarcode}")]
        public async Task<ActionResult<AdminProductAssociation>> GetAdminProductAssociation(string adminId, int productBarcode)
        {
            try
            {
                var adminProductAssociation = await _dbContext.AdminProductAssociations.FindAsync(adminId, productBarcode);

                if (adminProductAssociation == null)
                {
                    return NotFound(new { message = "AdminProductAssociation not found" });
                }

                return adminProductAssociation;
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Creates a new admin-product association.
        /// </summary>
        /// <param name="adminId">The ID of the admin.</param>
        /// <param name="productBarcode">The barcode of the product.</param>
        /// <param name="status">The status of the association.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("post/{adminId}/{productBarcode}")]
        public async Task<ActionResult<AdminProductAssociation>> CreateAdminProductAssociation(string adminId, int productBarcode, bool status)
        {
            try
            {
                var product0 = await _dbContext.Products.FindAsync(productBarcode);
                var admin = await _dbContext.Administrators.FindAsync(adminId);

                if (product0 == null || admin == null)
                {
                    return BadRequest(new { message = "Product or Admin does not exist!" });
                }

                var adminProductAssociation = new AdminProductAssociation();
                adminProductAssociation.Adminid = adminId;
                adminProductAssociation.Productbarcode = productBarcode;

                product0.Status = status;
                _dbContext.AdminProductAssociations.Add(adminProductAssociation);
                await _dbContext.SaveChangesAsync();

                var options = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                string json = JsonConvert.SerializeObject(adminProductAssociation, options);
                return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Updates an existing admin-product association. Este metodo no se usa porque esta tabla solo tiene PK compuesta por 2 elementos
        /// </summary>
        /// <param name="adminId">The ID of the admin.</param>
        /// <param name="productBarcode">The barcode of the product.</param>
        /// <param name="status">The new status of the association.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPut("put/{adminId}/{productBarcode}/{status}")]
        public async Task<IActionResult> UpdateAdminProductAssociation(string adminId, int productBarcode, bool status)
        {
            try
            {
                var adminProductAssociation = await _dbContext.AdminProductAssociations.FindAsync(adminId, productBarcode);
                var product0 = await _dbContext.Products.FindAsync(productBarcode);

                if (adminProductAssociation == null)
                {
                    return NotFound(new { message = "AdminProductAssociation not found" });
                }

                product0.Status = status;
                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Deletes an admin-product association.
        /// </summary>
        /// <param name="adminId">The ID of the admin.</param>
        /// <param name="productBarcode">The barcode of the product.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpDelete("delete/{adminId}/{productBarcode}")]
        public async Task<IActionResult> DeleteAdminProductAssociation(string adminId, int productBarcode)
        {
            try
            {
                var adminProductAssociation = await _dbContext.AdminProductAssociations.FindAsync(adminId, productBarcode);

                if (adminProductAssociation == null)
                {
                    return NotFound(new { message = "AdminProductAssociation not found" });
                }

                _dbContext.AdminProductAssociations.Remove(adminProductAssociation);
                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "ok" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
