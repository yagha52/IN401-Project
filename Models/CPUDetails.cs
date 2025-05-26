using System.ComponentModel.DataAnnotations;

namespace HardwareStore_Application.Models
{
    public class CPUDetails
    {
        public int CPUDetailsID { get; set; }

        [Required]
        public required Product Product { get; set; }

        public string? Socket { get; set; }
        public int? Cores { get; set; }
        public int? Threads { get; set; }
        public double? BaseClock { get; set; }
        public double? BoostClock { get; set; }
    }
}
