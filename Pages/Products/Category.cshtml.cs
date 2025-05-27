using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace HardwareStore_Application.Pages.Products
{
    public class CategoryModel : PageModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public string CategoryName { get; set; }
        public string SearchQuery { get; set; }

        public void OnGet(string categoryName, string brand, string query)
        {
            CategoryName = categoryName;
            SearchQuery = query;

            string connectionString = "server=localhost;user=root;password=R3in3.123@na;database=hardwarestore;";
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                int? categoryId = null;
                using (var cmd = new MySqlCommand("SELECT CategoryID FROM category WHERE CategoryName = @name", connection))
                {
                    cmd.Parameters.AddWithValue("@name", categoryName);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                        categoryId = Convert.ToInt32(result);
                }

                if (categoryId == null) return;

                string sql = @"
                    SELECT p.*, c.CategoryName, b.BrandName
                    FROM product p
                    JOIN category c ON p.CategoryID = c.CategoryID
                    JOIN brand b ON p.BrandID = b.BrandID
                    WHERE p.CategoryID = @catId";

                if (!string.IsNullOrEmpty(brand))
                    sql += " AND b.BrandName = @brand";

                if (!string.IsNullOrWhiteSpace(query))
                    sql += " AND p.ProductName LIKE @query";

                using (var cmd = new MySqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@catId", categoryId);
                    if (!string.IsNullOrEmpty(brand))
                        cmd.Parameters.AddWithValue("@brand", brand);
                    if (!string.IsNullOrWhiteSpace(query))
                        cmd.Parameters.AddWithValue("@query", $"%{query}%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Products.Add(new Product
                            {
                                ProductID = reader.GetInt32("ProductID"),
                                ProductName = reader.GetString("ProductName"),
                                CategoryID = reader.GetInt32("CategoryID"),
                                BrandID = reader.GetInt32("BrandID"),
                                OriginalPrice = reader.GetDecimal("OriginalPrice"),
                                Price = reader.GetDecimal("Price"),
                                image_url = reader.GetString("image_url")
                            });
                        }
                    }
                }
            }
        }

        public class Product
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }
            public int CategoryID { get; set; }
            public int BrandID { get; set; }
            public decimal OriginalPrice { get; set; }
            public decimal Price { get; set; }
            public string image_url { get; set; }
        }
    }
}
