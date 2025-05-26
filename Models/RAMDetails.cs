using System.ComponentModel.DataAnnotations;

namespace HardwareStore_Application.Models
{
    public class RAMDetails
    {
        public int RAMDetailsID { get; set; }
        public int ProductID { get; set; }
        
        [Required]
        public required Product Product { get; set; }
        
        [Required]
        public required string Type { get; set; }
        
        public int? Capacity { get; set; }
        public int? Speed { get; set; }
        public string? FormFactor { get; set; }
    }
}
