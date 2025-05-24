namespace HardwareStore_Application.Models
{
    public class CPUDetails
    {
        public int CPUDetailsID { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int Cores { get; set; }
        public int Threads { get; set; }
        public float BaseClockGHz { get; set; }
        public float BoostClockGHz { get; set; }
    }
}
