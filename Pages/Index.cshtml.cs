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
                 
                }
                catch (Exception ex)
                {
                
                    Console.WriteLine($"Database connection failed: {ex.Message}");
                }
            }
        }
    }
}