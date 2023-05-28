using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgre_API.Models;

namespace Postgre_API.Controllers
{
    [Route("api/PaymentType")]
    [ApiController]
    public class PaymentTypeController : ControllerBase
    {
        private readonly NutritecDbContext _dbContext;

        public PaymentTypeController(NutritecDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentType>>> GetPaymentTypes()
        {
            return await _dbContext.PaymentTypes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentType>> GetPaymentType(int id)
        {
            var paymentType = await _dbContext.PaymentTypes.FindAsync(id);

            if (paymentType == null)
            {
                return NotFound();
            }

            return paymentType;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentType>> CreatePaymentType(PaymentType paymentType)
        {
            _dbContext.PaymentTypes.Add(paymentType);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPaymentType), new { id = paymentType.Id }, paymentType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaymentType(int id, PaymentType paymentType)
        {
            if (id != paymentType.Id)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentType(int id)
        {
            var paymentType = await _dbContext.PaymentTypes.FindAsync(id);

            if (paymentType == null)
            {
                return NotFound();
            }

            _dbContext.PaymentTypes.Remove(paymentType);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentTypeExists(int id)
        {
            return _dbContext.PaymentTypes.Any(e => e.Id == id);
        }
    }
}
