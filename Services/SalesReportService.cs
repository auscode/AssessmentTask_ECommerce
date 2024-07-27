using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services
{
    public class SalesReportService
    {
        private readonly ApplicationDbContext _context;

        public SalesReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetDailyTotalRevenueAsync(DateTime date)
        {
            var sales = await _context.Sales
                .Where(s => s.SaleDate.Date == date.Date)
                .ToListAsync(); // Retrieve data first

            var totalRevenue = sales.Sum(s => s.TotalAmount); // Aggregate in-memory
            return totalRevenue;
        }

        public async Task<IEnumerable<ProductSalesReport>> GetMostSoldProductsAsync(DateTime date)
        {
            var sales = await _context.Sales
                .Where(s => s.SaleDate.Date == date.Date)
                .Include(s => s.SalesItems)
                .ThenInclude(si => si.Product)
                .ToListAsync(); // Retrieve data first

            var salesItems = sales.SelectMany(s => s.SalesItems).ToList();

            var productSalesReport = salesItems
                .GroupBy(si => si.ProductID)
                .Select(g => new ProductSalesReport
                {
                    ProductID = g.Key,
                    QuantitySold = g.Sum(si => si.Quantity),
                    TotalRevenue = g.Sum(si => si.Quantity * (decimal)salesItems.FirstOrDefault(si => si.ProductID == g.Key).Product.Price) // Aggregate in-memory
                })
                .OrderByDescending(psr => psr.QuantitySold)
                .ToList();

            return productSalesReport;
        }

        public class ProductSalesReport
        {
            public int ProductID { get; set; }
            public int QuantitySold { get; set; }
            public decimal TotalRevenue { get; set; }
        }
    }
}
