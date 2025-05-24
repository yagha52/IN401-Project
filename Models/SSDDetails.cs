namespace HardwareStore_Application.Models
{
    public class SSDDetails
    {
        public int SSDDetailsID { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }

        public int CapacityGB { get; set; }
        public string Type { get; set; } // e.g. SATA, NVMe
        public int ReadSpeedMBps { get; set; }
        public int WriteSpeedMBps { get; set; }
    }
}
