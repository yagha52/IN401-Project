using System.ComponentModel.DataAnnotations;

namespace HardwareStore_Application.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        
        [Required]
        public required string ProductName { get; set; }
        
        [Required]
        public required Category Category { get; set; }
        
        [Required]
        public required Brand Brand { get; set; }
        
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
    }
}
