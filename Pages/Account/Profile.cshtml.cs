using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace HardwareStore_Application.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        [BindProperty]
        public UserProfileViewModel UserProfile { get; set; } = new();

        [BindProperty]
        public AddressViewModel Address { get; set; } = new();

        public List<OrderViewModel> Orders { get; set; } = new();

        public string ActiveTab { get; set; } = "profile";

        public void OnGet(string tab = "profile")
        {
            ActiveTab = tab;
            LoadUserData();
            LoadAddressData();
            LoadOrderHistory();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index"); // or wherever you want to redirect after logout
        }

        public IActionResult OnPostUpdateProfile()
        {
            if (!ModelState.IsValid)
            {
                ActiveTab = "profile";
                LoadAddressData();
                LoadOrderHistory();
                return Page();
            }

            // Update user profile logic here
            // await _userService.UpdateProfileAsync(UserProfile);
            
            TempData["Success"] = "Profile updated successfully!";
            return RedirectToPage(new { tab = "profile" });
        }

        public IActionResult OnPostUpdateAddress()
        {
            if (!ModelState.IsValid)
            {
                ActiveTab = "address";
                LoadUserData();
                LoadOrderHistory();
                return Page();
            }

            // Update address logic here
            // await _addressService.UpdateAddressAsync(Address);
            
            TempData["Success"] = "Address updated successfully!";
            return RedirectToPage(new { tab = "address" });
        }

        private void LoadUserData()
        {
            // Load from database or service
            // Load from authenticated user claims
            if (User.Identity?.IsAuthenticated == true)
            {
                UserProfile = new UserProfileViewModel
                {
                    FullName = User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown User",
                    Email = User.FindFirst(ClaimTypes.Email)?.Value ?? "",
                    Phone = "+1 (555) 123-4567", // This would come from database
                    IsPremiumMember = bool.Parse(User.FindFirst("IsPremium")?.Value ?? "false")
                };
            }
        }

        private void LoadAddressData()
        {
            // Load from database or service
            Address = new AddressViewModel
            {
                StreetAddress = "123 Tech Street",
                City = "San Francisco",
                State = "CA",
                ZipCode = "94102",
                Country = "United States"
            };
        }

        private void LoadOrderHistory()
        {
            // Load from database or service
            Orders = new List<OrderViewModel>
            {
                new OrderViewModel
                {
                    OrderId = "ORD-001",
                    Status = "Delivered",
                    TotalAmount = 1299.98m,
                    OrderDate = new DateTime(2024, 1, 15),
                    Items = new List<OrderItemViewModel>
                    {
                        new OrderItemViewModel { Name = "AMD Ryzen 9 7950X", Quantity = 1, Price = 549.99m },
                        new OrderItemViewModel { Name = "NVIDIA RTX 4080 Super", Quantity = 1, Price = 749.99m }
                    }
                },
                new OrderViewModel
                {
                    OrderId = "ORD-002",
                    Status = "Shipped",
                    TotalAmount = 329.97m,
                    OrderDate = new DateTime(2024, 1, 8),
                    Items = new List<OrderItemViewModel>
                    {
                        new OrderItemViewModel { Name = "Corsair Vengeance DDR5-5600 32GB", Quantity = 1, Price = 179.99m },
                        new OrderItemViewModel { Name = "Samsung 980 PRO 2TB NVMe SSD", Quantity = 1, Price = 149.98m }
                    }
                }
            };
        }
    }

    public class UserProfileViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Phone")]
        public string Phone { get; set; } = string.Empty;

        public bool IsPremiumMember { get; set; }
    }

    public class AddressViewModel
    {
        [Required]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; } = string.Empty;

        [Required]
        [Display(Name = "City")]
        public string City { get; set; } = string.Empty;

        [Required]
        [Display(Name = "State")]
        public string State { get; set; } = string.Empty;

        [Required]
        [Display(Name = "ZIP Code")]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; } = string.Empty;
    }

    public class OrderViewModel
    {
        public string OrderId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new();
    }

    public class OrderItemViewModel
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}