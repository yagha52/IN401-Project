namespace HardwareStore_Application.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public int BrandID { get; set; }
        public Brand Brand { get; set; }

        public string image_url { get; set; }
    }
}
