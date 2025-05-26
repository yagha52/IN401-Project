using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Threading.Tasks;

namespace HardwareStore_Application.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;
        public string ConnectionString { get; private set; } = string.Empty;

        public IndexModel(IConfiguration config)
        {
            _config = config;
        }

        public async Task OnGet()
        {
            ConnectionString = _config.GetConnectionString("DefaultConnection") ?? string.Empty;

            if (!string.IsNullOrEmpty(ConnectionString))
            {
                try
                {
                    using var connection = new MySqlConnection(ConnectionString);
                    await connection.OpenAsync();
                    // Database connection successful
                    // You can add logic here to fetch featured products, categories, etc.
                }
                catch (Exception ex)
                {
                    // Log the error or handle it appropriately
                    // For now, we'll continue without database data
                    Console.WriteLine($"Database connection failed: {ex.Message}");
                }
            }
        }
    }
}