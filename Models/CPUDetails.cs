using System.ComponentModel.DataAnnotations;

namespace HardwareStore_Application.Models
{
    public class CPUDetails
    {
        public int ProductID { get; set; }

        public int? Cores { get; set; }
        public int? Threads { get; set; }
        public double? BaseClockGHz { get; set; }
        public double? BoostClockGHz { get; set; }
        public string? Socket { get; set; }
    }
}