using E_Commerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesReportController : ControllerBase
    {
        private readonly SalesReportService _salesReportService;

        public SalesReportController(SalesReportService salesReportService)
        {
            _salesReportService = salesReportService;
        }

        [HttpGet("daily-revenue")]
        public async Task<ActionResult<decimal>> GetDailyTotalRevenue(DateTime date)
        {
            var totalRevenue = await _salesReportService.GetDailyTotalRevenueAsync(date);
            return Ok(totalRevenue);
        }

        [HttpGet("most-sold-products")]
        public async Task<ActionResult<IEnumerable<SalesReportService>>> GetMostSoldProducts(DateTime date)
        {
            var mostSoldProducts = await _salesReportService.GetMostSoldProductsAsync(date);
            return Ok(mostSoldProducts);
        }
    }
}
