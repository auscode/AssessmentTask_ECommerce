namespace E_Commerce.DTO
{
    public class SalesDTO
    {
        public int SaleID { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime SaleDate { get; set; }
        public List<SalesItemDTO> SalesItems { get; set; }
    }
}
