using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace HardwareStore_Application.Pages.Products
{
    [IgnoreAntiforgeryToken]
    [Route("/Products/Category/{categoryName}")]
    public class CategoryModel : PageModel
    {
        public List<Product> Products { get; set; } = new();

        [FromRoute(Name = "categoryName")]
        public string CategoryName { get; set; }

        [FromQuery(Name = "brand")]
        public string Brand { get; set; }

        public void OnGet()
        {
            string connectionString = "server=localhost;port=3306;database=hardwarestore;user=root;password=R3in3.123@na;";
            using MySqlConnection conn = new(connectionString);
            conn.Open();

            string query = @"
                SELECT p.ProductID, p.ProductName, p.BrandID, p.Price, p.OriginalPrice
                FROM product p
                JOIN category c ON p.CategoryID = c.CategoryID
                JOIN brand b ON p.BrandID = b.BrandID
                WHERE c.CategoryName = @categoryName
            ";

            if (!string.IsNullOrEmpty(Brand))
            {
                query += " AND b.BrandName = @brand";
            }

            using MySqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@categoryName", CategoryName);

            if (!string.IsNullOrEmpty(Brand))
            {
                cmd.Parameters.AddWithValue("@brand", Brand);
            }

            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Products.Add(new Product
                {
                    ProductID = reader.GetInt32("ProductID"),
                    ProductName = reader.GetString("ProductName"),
                    BrandID = reader.GetInt32("BrandID"),
                    Price = reader.GetDecimal("Price"),
                    OriginalPrice = reader.IsDBNull(reader.GetOrdinal("OriginalPrice")) ? 0 : reader.GetDecimal("OriginalPrice"),
                });
            }
        }

        public class Product
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public int BrandID { get; set; }
            public decimal Price { get; set; }
            public decimal OriginalPrice { get; set; }
        }
    }
}
