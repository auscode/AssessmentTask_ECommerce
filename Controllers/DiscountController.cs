using E_Commerce.Models;
using E_Commerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly DiscountService _discountService;

        public DiscountController(DiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Discount>>> GetDiscounts()
        {
            var discounts = await _discountService.GetDiscountsAsync();
            return Ok(discounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Discount>> GetDiscount(int id)
        {
            var discount = await _discountService.GetDiscountByIdAsync(id);
            if (discount == null)
            {
                return NotFound();
            }
            return Ok(discount);
        }

        [HttpPost]
        public async Task<ActionResult<Discount>> PostDiscount(Discount discount)
        {
            var createdDiscount = await _discountService.AddDiscountAsync(discount);
            return CreatedAtAction(nameof(GetDiscount), new { id = createdDiscount.DiscountID }, createdDiscount);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiscount(int id, Discount discount)
        {
            if (id != discount.DiscountID)
            {
                return BadRequest();
            }

            try
            {
                await _discountService.UpdateDiscountAsync(discount);
            }
            catch (Exception)
            {
                if (await _discountService.GetDiscountByIdAsync(id) == null)
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
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var result = await _discountService.DeleteDiscountAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
