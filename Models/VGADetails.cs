namespace HardwareStore_Application.Models
{
    public class VGADetails
    {
        public int VGADetailsID { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int MemoryGB { get; set; }
        public int CoreClockMHz { get; set; }
        public int BoostClockMHz { get; set; }
    }
}
