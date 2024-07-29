using E_Commerce.DTO;
using E_Commerce.Models;
using E_Commerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly SalesService _salesService;
        private readonly CartService _cartService;

        public SalesController(SalesService salesService, CartService cartService)
        {
            _salesService = salesService;
            _cartService = cartService;
        }

        [HttpPost("report")]
        public async Task<IActionResult> CreateSalesReport([FromBody] List<CartItemDTO> cartItems)
        {
            if (cartItems == null || !cartItems.Any())
            {
                return BadRequest("No cart items provided.");
            }

            try
            {
                var result = await _salesService.CreateSalesReportAsync(cartItems);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception details if necessary
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("total-revenue")]
        public async Task<ActionResult<decimal>> GetTotalRevenueForDay()
        {
            var totalRevenue = await _salesService.GetTotalRevenueForDayAsync(DateTime.Now);
            return Ok(totalRevenue);
        }

        [HttpGet("most-sold-products")]
        public async Task<ActionResult<List<Product>>> GetMostSoldProducts()
        {
            var mostSoldProducts = await _salesService.GetMostSoldProductsAsync(DateTime.Now);
            return Ok(mostSoldProducts);
        }
        [HttpGet("all-sales-items")]
        public async Task<ActionResult<List<SalesItemDTO>>> GetAllSalesItems()
        {
            try
            {
                var salesItems = await _salesService.GetAllSalesItemsAsync();
                return Ok(salesItems);
            }
            catch (Exception ex)
            {
                // Log the exception details if necessary
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
