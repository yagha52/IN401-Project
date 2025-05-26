using System.ComponentModel.DataAnnotations;

namespace HardwareStore_Application.Models
{
    public class Inventory
    {
        public int InventoryID { get; set; }
        [Required]
        public required Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
