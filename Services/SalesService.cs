using E_Commerce.DTO;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Services
{
    public class SalesService
    {
        private readonly ApplicationDbContext _context;

        public SalesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SalesDTO> CreateSalesReportAsync(List<CartItemDTO> cartItemDTOs)
        {
            var cartItems = new List<CartItem>();

            foreach (var dto in cartItemDTOs)
            {
                var product = await _context.Products.FindAsync(dto.ProductID);
                if (product != null)
                {
                    cartItems.Add(new CartItem
                    {
                        ProductID = dto.ProductID,
                        Quantity = dto.Quantity,
                        Product = product
                    });
                }
            }

            var sale = new Sale
            {
                SaleDate = DateTime.Now,
                TotalAmount = cartItems.Sum(ci => ci.Quantity * ci.Product.Price)
            };

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            var salesItems = cartItems.Select(ci => new SalesItem
            {
                SaleID = sale.SaleID,
                ProductID = ci.ProductID,
                Quantity = ci.Quantity,
                Price = ci.Product.Price
            }).ToList();

            _context.SalesItems.AddRange(salesItems);
            await _context.SaveChangesAsync();

            return new SalesDTO
            {
                SaleID = sale.SaleID,
                TotalAmount = sale.TotalAmount,
                SaleDate = sale.SaleDate,
                SalesItems = salesItems.Select(si => new SalesItemDTO
                {
                    SalesItemID = si.SalesItemID,
                    SaleID = si.SaleID,
                    ProductID = si.ProductID,
                    Quantity = si.Quantity,
                    Price = si.Price
                }).ToList()
            };
        }
        public async Task<decimal> GetTotalRevenueForDayAsync(DateTime date)
        {
            return await _context.Sales
                .Where(s => s.SaleDate.Date == date.Date)
                .SumAsync(s => s.TotalAmount);
        }

        public async Task<List<Product>> GetMostSoldProductsAsync(DateTime date)
        {
            return await _context.SalesItems
                .Where(si => _context.Sales.Any(s => s.SaleID == si.SaleID && s.SaleDate.Date == date.Date))
                .GroupBy(si => si.ProductID)
                .Select(g => new
                {
                    Product = _context.Products.Find(g.Key),
                    Quantity = g.Sum(si => si.Quantity)
                })
                .OrderByDescending(x => x.Quantity)
                .Select(x => x.Product)
                .ToListAsync();
        }
        public async Task<List<SalesItemDTO>> GetAllSalesItemsAsync()
        {
            return await _context.SalesItems
                .Select(si => new SalesItemDTO
                {
                    SalesItemID = si.SalesItemID,
                    SaleID = si.SaleID,
                    ProductID = si.ProductID,
                    Quantity = si.Quantity,
                    // Price = si.Price
                })
                .ToListAsync();
        }
    }
}
