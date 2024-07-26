namespace E_Commerce.Models;

public class Product
{
    public int ProductID { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public ICollection<CartItem> CartItems { get; set; }
    public ICollection<SalesItem> SalesItems { get; set; }
}