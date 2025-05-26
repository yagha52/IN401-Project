using System.ComponentModel.DataAnnotations;

namespace HardwareStore_Application.Models
{
    public class VGADetails
    {
        public int VGADetailsID { get; set; }

        [Required]
        public required Product Product { get; set; }

        public int MemoryGB { get; set; }
        public int CoreClockMHz { get; set; }
        public int BoostClockMHz { get; set; }

        public string? MemoryType { get; set; }
        public int? MemorySize { get; set; }
        public string? Interface { get; set; }
    }
}
