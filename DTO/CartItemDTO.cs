namespace E_Commerce.DTO
{
    public class CartItemDTO
    {
        public int CartItemID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public ProductDTO Product { get; set; }
    }
}
