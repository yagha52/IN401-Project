using System.ComponentModel.DataAnnotations;

namespace HardwareStore_Application.Models
{
    public class CPUDetails
    {
        [Required]
        public required Product Product { get; set; }

        public string? Socket { get; set; }
        public int? Cores { get; set; }
        public int? Threads { get; set; }
        public double? BaseClockGHz { get; set; }
        public double? BoostClockGHz { get; set; }
    }
}