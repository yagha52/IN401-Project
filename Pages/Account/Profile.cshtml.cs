using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace HardwareStore_Application.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly HardwareStoreDbContext _context;
        private readonly ILogger<ProfileModel> _logger;

        public ProfileModel(HardwareStoreDbContext context, ILogger<ProfileModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public UserProfileViewModel UserProfile { get; set; } = new();

        [BindProperty]
        public AddressViewModel Address { get; set; } = new();

        public List<OrderViewModel> Orders { get; set; } = new();

        public string ActiveTab { get; set; } = "profile";

        public async Task OnGetAsync(string tab = "profile")
        {
            ActiveTab = tab;
            await LoadUserDataAsync();
            await LoadOrderHistoryAsync();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostUpdateProfileAsync()
        {
            if (!ModelState.IsValid)
            {
                ActiveTab = "profile";
                await LoadOrderHistoryAsync();
                return Page();
            }

            try
            {
                var customerId = GetCurrentCustomerId();
                if (customerId == 0)
                {
                    TempData["Error"] = "User session expired. Please login again.";
                    return RedirectToPage("/Account/Login");
                }

                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
                
                if (customer != null)
                {
                    // Split FullName into FirstName and LastName
                    var nameParts = UserProfile.FullName.Trim().Split(' ', 2);
                    customer.FirstName = nameParts[0];
                    customer.LastName = nameParts.Length > 1 ? nameParts[1] : "";
                    customer.Email = UserProfile.Email;
                    customer.Phone = UserProfile.Phone;

                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = "Profile updated successfully!";
                    _logger.LogInformation("Profile updated for customer ID: {CustomerId}", customerId);
                }
                else
                {
                    TempData["Error"] = "User not found.";
                    _logger.LogWarning("Customer not found with ID: {CustomerId}", customerId);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while updating your profile. Please try again.";
                _logger.LogError(ex, "Error updating profile for customer ID: {CustomerId}", GetCurrentCustomerId());
            }

            return RedirectToPage(new { tab = "profile" });
        }

        public async Task<IActionResult> OnPostUpdateAddressAsync()
        {
            // Validate the address model state specifically
            ModelState.Clear();
            TryValidateModel(Address, nameof(Address));
            
            if (!ModelState.IsValid)
            {
                ActiveTab = "address";
                await LoadUserDataAsync();
                await LoadOrderHistoryAsync();
                return Page();
            }

            try
            {
                var customerId = GetCurrentCustomerId();
                if (customerId == 0)
                {
                    TempData["Error"] = "User session expired. Please login again.";
                    return RedirectToPage("/Account/Login");
                }

                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
                
                if (customer != null)
                {
                    // Update address fields
                    customer.StreetAddress = string.IsNullOrWhiteSpace(Address.StreetAddress) ? null : Address.StreetAddress.Trim();
                    customer.City = string.IsNullOrWhiteSpace(Address.City) ? null : Address.City.Trim();
                    customer.State = string.IsNullOrWhiteSpace(Address.State) ? null : Address.State.Trim();
                    customer.ZipCode = string.IsNullOrWhiteSpace(Address.ZipCode) ? null : Address.ZipCode.Trim();
                    customer.Country = string.IsNullOrWhiteSpace(Address.Country) ? null : Address.Country.Trim();

                    // Save changes to database
                    var rowsAffected = await _context.SaveChangesAsync();
                    
                    if (rowsAffected > 0)
                    {
                        TempData["Success"] = "Address updated successfully!";
                        _logger.LogInformation("Address updated for customer ID: {CustomerId}", customerId);
                    }
                    else
                    {
                        TempData["Info"] = "No changes were made to your address.";
                    }
                }
                else
                {
                    TempData["Error"] = "User not found.";
                    _logger.LogWarning("Customer not found with ID: {CustomerId}", customerId);
                }
            }
            catch (DbUpdateException ex)
            {
                TempData["Error"] = "Database error occurred while updating your address. Please try again.";
                _logger.LogError(ex, "Database error updating address for customer ID: {CustomerId}", GetCurrentCustomerId());
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occurred while updating your address. Please try again.";
                _logger.LogError(ex, "Unexpected error updating address for customer ID: {CustomerId}", GetCurrentCustomerId());
            }

            return RedirectToPage(new { tab = "address" });
        }

        private async Task LoadUserDataAsync()
        {
            try
            {
                var customerId = GetCurrentCustomerId();
                if (customerId == 0)
                {
                    _logger.LogWarning("Invalid customer ID when loading user data");
                    return;
                }

                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
                
                if (customer != null)
                {
                    UserProfile = new UserProfileViewModel
                    {
                        FullName = customer.FullName,
                        Email = customer.Email,
                        Phone = customer.Phone ?? ""
                    };

                    Address = new AddressViewModel
                    {
                        StreetAddress = customer.StreetAddress ?? "",
                        City = customer.City ?? "",
                        State = customer.State ?? "",
                        ZipCode = customer.ZipCode ?? "",
                        Country = customer.Country ?? ""
                    };
                }
                else
                {
                    _logger.LogWarning("Customer not found with ID: {CustomerId}", customerId);
                    // Initialize with empty data if customer not found
                    UserProfile = new UserProfileViewModel();
                    Address = new AddressViewModel();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user data for customer ID: {CustomerId}", GetCurrentCustomerId());
                // Initialize with empty data in case of error
                UserProfile = new UserProfileViewModel();
                Address = new AddressViewModel();
            }
        }

        private async Task LoadOrderHistoryAsync()
        {
            try
            {
                var customerId = GetCurrentCustomerId();
                if (customerId == 0)
                {
                    Orders = new List<OrderViewModel>();
                    return;
                }
                
                var orders = await _context.Orders
                    .Where(o => o.CustomerId == customerId)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product) // Include product to get name
                    .OrderByDescending(o => o.OrderDate)
                    .Select(o => new OrderViewModel
                    {
                        OrderId = o.OrderId.ToString(),
                        Status = "Delivered", // You might want to add a Status field to your Order model
                        TotalAmount = o.TotalAmount,
                        OrderDate = o.OrderDate,
                        Items = o.OrderItems.Select(oi => new OrderItemViewModel
                        {
                            Name = oi.Product != null ? oi.Product.ProductName : "Unknown Product",
                            Quantity = oi.Quantity,
                            Price = oi.Price
                        }).ToList()
                    })
                    .ToListAsync();

                Orders = orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading order history for customer ID: {CustomerId}", GetCurrentCustomerId());
                Orders = new List<OrderViewModel>();
            }
        }

        private int GetCurrentCustomerId()
        {
            try
            {
                // Get customer ID from claims
                var customerIdClaim = User.FindFirst("CustomerId")?.Value;
                if (int.TryParse(customerIdClaim, out int customerId))
                {
                    return customerId;
                }
                
                // Fallback: try to get from database using email
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (!string.IsNullOrEmpty(email))
                {
                    var customer = _context.Customers.FirstOrDefault(c => c.Email == email);
                    return customer?.CustomerId ?? 0;
                }
                
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current customer ID");
                return 0;
            }
        }
    }

    public class UserProfileViewModel
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        [Display(Name = "Phone")]
        public string Phone { get; set; } = string.Empty;
    }

    public class AddressViewModel
    {
        [StringLength(200, ErrorMessage = "Street address cannot exceed 200 characters")]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        [Display(Name = "City")]
        public string City { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "State cannot exceed 50 characters")]
        [Display(Name = "State")]
        public string State { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "ZIP code cannot exceed 20 characters")]
        [Display(Name = "ZIP Code")]
        public string ZipCode { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters")]
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