using System.ComponentModel.DataAnnotations;

namespace HardwareStore_Application.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public string SessionId { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public decimal TotalPrice => Price * Quantity;
    }
}