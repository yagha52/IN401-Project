using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using HardwareStore.Models;
using System.Data;

namespace HardwareStore_Application.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly string _connectionString = "server=localhost;port=3306;database=hardwarestore;user=root;password=;";

        public LoginModel(ILogger<LoginModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public LoginViewModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? "/";
            
            // Clear any existing authentication
            if (HttpContext.User.Identity?.IsAuthenticated == true)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            
            // Optional: Load any data needed for the login page
            // For example, you could load login attempts, promotional messages, etc.
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();
                
                // Example: Get count of registered users (for display purposes)
                string countQuery = "SELECT COUNT(*) FROM customer";
                using var countCommand = new MySqlCommand(countQuery, connection);
                var userCount = Convert.ToInt32(await countCommand.ExecuteScalarAsync());
                
                ViewData["UserCount"] = userCount;
                _logger.LogInformation("Login page loaded. Total registered users: {UserCount}", userCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading login page data");
                ViewData["UserCount"] = 0;
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= "/";

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Validate user credentials against database
            var customer = await ValidateUserAsync(Input.Email, Input.Password);

            if (customer == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return Page();
            }

            // Create claims for the user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customer.FullName),
                new Claim(ClaimTypes.Email, customer.Email),
                new Claim("CustomerId", customer.CustomerId.ToString()),
                new Claim("FirstName", customer.FirstName),
                new Claim("LastName", customer.LastName)
            };

            if (!string.IsNullOrEmpty(customer.Phone))
            {
                claims.Add(new Claim("PhoneNumber", customer.Phone));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = Input.RememberMe,
                ExpiresUtc = Input.RememberMe ?
                    DateTimeOffset.UtcNow.AddDays(30) :
                    DateTimeOffset.UtcNow.AddHours(2)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                authProperties);

            _logger.LogInformation("User {Email} logged in successfully", Input.Email);

            return LocalRedirect(returnUrl);
        }

        private async Task<Customer?> ValidateUserAsync(string email, string password)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();
                _logger.LogInformation("Database connection opened successfully");

                // Use case-insensitive email comparison
                string query = @"
                    SELECT CustomerId, FirstName, LastName, Email, Phone, PasswordHash, 
                           StreetAddress, City, State, ZipCode, Country
                    FROM customer 
                    WHERE LOWER(Email) = LOWER(@email)";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);

                _logger.LogInformation("Searching for user with email: {Email}", email);

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var storedHash = reader.GetString("PasswordHash");
                    _logger.LogInformation("User found. Stored password hash: {Hash}", storedHash);
                    _logger.LogInformation("Input password: {Password}", password);

                    // First, try direct comparison (for plain text passwords)
                    if (password == storedHash)
                    {
                        _logger.LogInformation("Password matched directly (plain text)");
                        return CreateCustomerFromReader((MySqlDataReader)reader, storedHash);
                    }

                    // Then try hashed comparison
                    var inputHash = HashPassword(password);
                    _logger.LogInformation("Hashed input password: {InputHash}", inputHash);

                    if (inputHash == storedHash)
                    {
                        _logger.LogInformation("Password matched after hashing");
                        return CreateCustomerFromReader((MySqlDataReader)reader, storedHash);
                    }

                    _logger.LogWarning("Password verification failed for email: {Email}. Neither plain text nor hashed comparison worked.", email);
                }
                else
                {
                    _logger.LogWarning("No user found with email: {Email}", email);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating user credentials for {Email}", email);
                return null;
            }
        }

        private Customer CreateCustomerFromReader(MySqlDataReader reader, string storedHash)
        {
            return new Customer
            {
                CustomerId = reader.GetInt32("CustomerId"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Email = reader.GetString("Email"),
                Phone = reader.IsDBNull("Phone") ? null : reader.GetString("Phone"),
                PasswordHash = storedHash,
                StreetAddress = reader.IsDBNull("StreetAddress") ? null : reader.GetString("StreetAddress"),
                City = reader.IsDBNull("City") ? null : reader.GetString("City"),
                State = reader.IsDBNull("State") ? null : reader.GetString("State"),
                ZipCode = reader.IsDBNull("ZipCode") ? null : reader.GetString("ZipCode"),
                Country = reader.IsDBNull("Country") ? null : reader.GetString("Country")
            };
        }

        private bool VerifyPassword(string password, string hash)
        {
            // Simple hash verification - in production, use BCrypt or similar
            var inputHash = HashPassword(password);
            return inputHash == hash;
        }

        private string HashPassword(string password)
        {
            // Simple SHA256 hash - in production, use BCrypt with salt
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}