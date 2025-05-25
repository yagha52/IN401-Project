using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace HardwareStore_Application.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterViewModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }

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
            var existingUser = await CheckUserExistsAsync(Input.Email);
            if (existingUser)
            {
                ModelState.AddModelError(string.Empty, "An account with this email already exists.");
                return Page();
            }

            // Create new user (replace with your actual user creation logic)
            var newUser = await CreateUserAsync(Input);
            
            if (newUser == null)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating your account. Please try again.");
                return Page();
            }

            // Sign in the new user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, newUser.FullName),
                new Claim(ClaimTypes.Email, newUser.Email),
                new Claim("UserId", newUser.Id.ToString()),
                new Claim("IsPremium", "false") // New users start as regular members
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                });

            TempData["Success"] = "Welcome! Your account has been created successfully.";
            return LocalRedirect(returnUrl);
        }

        private async Task<bool> CheckUserExistsAsync(string email)
        {
            // Replace with your actual user existence check
            // This could be a database query, API call, etc.
            
            // Mock check - replace with real implementation
            var existingEmails = new[] 
            { 
                "john.doe@example.com", 
                "user@test.com",
                "admin@techstore.com"
            };
            
            return existingEmails.Contains(email.ToLower());
        }

        private async Task<UserDto?> CreateUserAsync(RegisterViewModel model)
        {
            try
            {
                // Replace with your actual user creation logic
                // Hash password, save to database, etc.
                
                // Mock user creation - replace with real implementation
                var newUser = new UserDto
                {
                    Id = new Random().Next(1000, 9999), // Generate random ID for demo
                    FullName = model.FullName,
                    Email = model.Email,
                    IsPremiumMember = false
                };

                // Here you would:
                // 1. Hash the password using BCrypt or similar
                // 2. Save user to database
                // 3. Send welcome email
                // 4. Create user profile records
                
                await Task.Delay(100); // Simulate async operation
                
                return newUser;
            }
            catch (Exception)
            {
                // Log the exception
                return null;
            }
        }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the Terms and Conditions.")]
        [Display(Name = "I agree to the Terms and Conditions")]
        public bool AcceptTerms { get; set; }

        [Display(Name = "Subscribe to newsletter")]
        public bool SubscribeNewsletter { get; set; }
    }
}