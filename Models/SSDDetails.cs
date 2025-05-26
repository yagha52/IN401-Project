using System.ComponentModel.DataAnnotations;

namespace HardwareStore_Application.Models
{
    public class SSDDetails
    {
        public int SSDDetailsID { get; set; }

        [Required]
        public required Product Product { get; set; }

        [Required]
        public required string Type { get; set; } // e.g. SATA, NVMe

        public int? CapacityGB { get; set; }
        public string? Interface { get; set; }
        public int? ReadSpeedMBps { get; set; }
        public int? WriteSpeedMBps { get; set; }
    }
}
