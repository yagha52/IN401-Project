using System.ComponentModel.DataAnnotations;

namespace HardwareStore_Application.Models
{
    public class Brand
    {
        public int BrandId { get; set; }
        
        [Required]
        public required string BrandName { get; set; }
    }
}