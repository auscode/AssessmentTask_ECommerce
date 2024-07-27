using E_Commerce.Models;

namespace E_Commerce.Services
{
    public interface ISalesItemService
    {
        SalesItem CreateSalesItem(SalesItemCreateDTO salesItemDto);
    }
}
