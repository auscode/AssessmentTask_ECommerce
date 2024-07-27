using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class SalesItemCreateDTO
    {
        [Required]
        public int SaleID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
