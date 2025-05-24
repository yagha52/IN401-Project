namespace HardwareStore_Application.Models
{
    public class RAMDetails
    {
        public int RAMDetailsID { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int CapacityGB { get; set; }
        public int SpeedMHz { get; set; }
        public string Type { get; set; }
    }
}
