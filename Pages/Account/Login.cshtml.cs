using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace HardwareStore_Application.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginViewModel Input { get; set; } = new();

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

            // Validate user credentials (replace with your actual authentication logic)
            var user = await ValidateUserAsync(Input.Email, Input.Password);
            
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return Page();
            }

            // Create claims for the user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.Id.ToString()),
                new Claim("IsPremium", user.IsPremiumMember.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                new AuthenticationProperties
                {
                    IsPersistent = Input.RememberMe,
                    ExpiresUtc = Input.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(2)
                });

            return LocalRedirect(returnUrl);
        }

        private Task<UserDto?> ValidateUserAsync(string email, string password)
        {
            // Replace this with your actual user validation logic
            // This could be a database call, API call, etc.
            
            // Mock validation - replace with real authentication
            if (email == "john.doe@example.com" && password == "password123")
            {
                return Task.FromResult<UserDto?>(new UserDto
                {
                    Id = 1,
                    FullName = "John Doe",
                    Email = email,
                    IsPremiumMember = true
                });
            }
            
            // Add more mock users for testing
            if (email == "user@test.com" && password == "test123")
            {
                return Task.FromResult<UserDto?>(new UserDto
                {
                    Id = 2,
                    FullName = "Test User",
                    Email = email,
                    IsPremiumMember = false
                });
            }

            return Task.FromResult<UserDto?>(null); // Invalid credentials
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

    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsPremiumMember { get; set; }
    }
}