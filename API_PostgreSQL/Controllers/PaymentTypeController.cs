using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgre_API.Models;

namespace Postgre_API.Controllers
{
    /// <summary>
    /// Controller for managing payment types in the API.
    /// </summary>
    [Route("api/PaymentType")]
    [ApiController]
    public class PaymentTypeController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypeController"/> class.
        /// </summary>
        /// <param name="dbContext">The Nutritec database context.</param>
        public PaymentTypeController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves all payment types.
        /// </summary>
        /// <returns>A list of payment types.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentType>>> GetPaymentTypes()
        {
            try{
            return await _dbContext.PaymentTypes.ToListAsync();
            }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}

        /// <summary>
        /// Retrieves a specific payment type by ID.
        /// </summary>
        /// <param name="id">The ID of the payment type to retrieve.</param>
        /// <returns>The payment type with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentType>> GetPaymentType(int id)
        {
            try{
            var paymentType = await _dbContext.PaymentTypes.FindAsync(id);

            if (paymentType == null)
            {
                return NotFound(new { message = "Payment type not found" });
            }

            return paymentType;
        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}

        /// <summary>
        /// Creates a new payment type.
        /// </summary>
        /// <param name="id">The ID of the payment type.</param>
        /// <param name="descripcion">The description of the payment type.</param>
        /// <returns>The created payment type.</returns>
        [HttpPost("post/{descripcion}")]
        public async Task<IActionResult> CreatePaymentType(int id, string descripcion)
        {
            try{
            var exists_payment = await _dbContext.PaymentTypes.FindAsync(id);

            if (exists_payment != null)
            {
                return BadRequest(new {message = "The payment type already exists!"});
            }

            var myPay = new PaymentType { Id = id, Description = descripcion };

            _dbContext.PaymentTypes.Add(myPay);
            await _dbContext.SaveChangesAsync();

            return Ok(new {message = "ok"});
        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}

        /// <summary>
        /// Updates an existing payment type.
        /// </summary>
        /// <param name="id">The ID of the payment type to update.</param>
        /// <param name="paymentType">The updated payment type object.</param>
        /// <returns>A response indicating the success of the update operation.</returns>
        [HttpPut("put/{id}")]
        public async Task<IActionResult> UpdatePaymentType(int id, PaymentType paymentType)
        {
            try{
            if (id != paymentType.Id)
            {
                return BadRequest(new { message = "Invalid request" });
            }

            _dbContext.Entry(paymentType).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentTypeExists(id))
                {
                    return NotFound(new { message = "Payment type not found" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new {message = "ok"});
        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}

        /// <summary>
        /// Deletes a payment type.
        /// </summary>
        /// <param name="id">The ID of the payment type to delete.</param>
        /// <returns>A response indicating the success of the delete operation.</returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletePaymentType(int id)
        {
            try{
            var paymentType = await _dbContext.PaymentTypes.FindAsync(id);

            if (paymentType == null)
            {
                return NotFound(new { message = "Payment type not found" });
            }

            _dbContext.PaymentTypes.Remove(paymentType);
            await _dbContext.SaveChangesAsync();

            return Ok(new {message = "ok"});
            
        }catch (Exception e)
            {
                return BadRequest(new {message = e.Message});
            }}

        private bool PaymentTypeExists(int id)
        {
            return _dbContext.PaymentTypes.Any(e => e.Id == id);
        }
    }
}
