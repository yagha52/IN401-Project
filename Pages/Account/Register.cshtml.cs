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

namespace HardwareStore_Application.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;
        private readonly string _connectionString = "server=localhost;port=3306;database=hardwarestore;user=root;password=;";

        public RegisterModel(ILogger<RegisterModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public RegisterViewModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= "/";

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if user already exists
            if (await UserExistsAsync(Input.Email))
            {
                ModelState.AddModelError(string.Empty, "An account with this email address already exists.");
                return Page();
            }

            // Create new user in database
            var newCustomer = await CreateUserAsync(Input);
            
            if (newCustomer != null)
            {
                // Create claims for the new user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, newCustomer.FullName),
                    new Claim(ClaimTypes.Email, newCustomer.Email),
                    new Claim("CustomerId", newCustomer.CustomerId.ToString()),
                    new Claim("FirstName", newCustomer.FirstName),
                    new Claim("LastName", newCustomer.LastName),
                };

                if (!string.IsNullOrEmpty(newCustomer.Phone))
                {
                    claims.Add(new Claim("PhoneNumber", newCustomer.Phone));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false, // Don't remember registration login
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    authProperties);

                _logger.LogInformation("New user registered: {Email}", Input.Email);

                Message = "Account created successfully! Welcome to Hardware Store.";
                IsSuccess = true;

                // Redirect after successful registration
                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Failed to create account. Please try again.");
            return Page();
        }

        private async Task<bool> UserExistsAsync(string email)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                string query = "SELECT COUNT(*) FROM customer WHERE Email = @email";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);

                var count = Convert.ToInt32(await command.ExecuteScalarAsync());
                return count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if user exists for email {Email}", email);
                return true; // Assume exists to prevent duplicate attempts on error
            }
        }

        private async Task<Customer?> CreateUserAsync(RegisterViewModel input)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                // Parse full name into first and last name
                var nameParts = input.FullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var firstName = nameParts.Length > 0 ? nameParts[0] : input.FullName;
                var lastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";

                // Hash the password
                var passwordHash = HashPassword(input.Password);

                string insertQuery = @"
                    INSERT INTO customer (FirstName, LastName, Email, Phone, PasswordHash) 
                    VALUES (@firstName, @lastName, @email, @phone, @passwordHash);
                    SELECT LAST_INSERT_ID();";

                using var command = new MySqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@email", input.Email);
                command.Parameters.AddWithValue("@phone", input.PhoneNumber ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@passwordHash", passwordHash);

                var customerId = Convert.ToInt32(await command.ExecuteScalarAsync());

                // Return the created customer
                return new Customer
                {
                    CustomerId = customerId,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = input.Email,
                    Phone = input.PhoneNumber,
                    PasswordHash = passwordHash,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user account for {Email}", input.Email);
                return null;
            }
        }

        private string HashPassword(string password)
        {
            // Simple SHA256 hash - in production, use BCrypt with salt
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The password must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", 
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}