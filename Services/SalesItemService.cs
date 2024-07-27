using E_Commerce.Models;
using System.Linq;

namespace E_Commerce.Services
{
    public class SalesItemService : ISalesItemService
    {
        private readonly ApplicationDbContext _context;

        public SalesItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public SalesItem CreateSalesItem(SalesItemCreateDTO salesItemDto)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var salesItem = new SalesItem
                {
                    SaleID = salesItemDto.SaleID,
                    ProductID = salesItemDto.ProductID,
                    Quantity = salesItemDto.Quantity
                };

                _context.SalesItems.Add(salesItem);
                _context.SaveChanges();

                transaction.Commit();

                return salesItem;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                // Log the exception or handle it as needed
                throw new InvalidOperationException("Error while creating SalesItem.", ex);
            }
        }

    }
}
