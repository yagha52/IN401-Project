namespace HardwareStore_Application.Models
{
    public class SSDDetails
    {

        public int ProductID { get; set; }
        public int CapacityGB { get; set; }
        public string Type { get; set; } // e.g. SATA, NVMe
        public string? Interface { get; set; }

    }
}
