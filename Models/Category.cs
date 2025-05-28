using System.ComponentModel.DataAnnotations;

namespace HardwareStore_Application.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        
        [Required]
        public required string CategoryName { get; set; }
    }
}