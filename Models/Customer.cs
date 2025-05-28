using System.ComponentModel.DataAnnotations;

namespace HardwareStore.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string? Phone { get; set; }

        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Street address cannot exceed 200 characters")]
        public string? StreetAddress { get; set; }

        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        public string? City { get; set; }

        [StringLength(50, ErrorMessage = "State cannot exceed 50 characters")]
        public string? State { get; set; }

        [StringLength(20, ErrorMessage = "Zip code cannot exceed 20 characters")]
        public string? ZipCode { get; set; }

        [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters")]
        public string? Country { get; set; }

        // Navigation property for orders
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        // For display purposes
        public string FullName => $"{FirstName} {LastName}";
        public string FullAddress => BuildFullAddress();

        private string BuildFullAddress()
        {
            var addressParts = new List<string>();
            
            if (!string.IsNullOrWhiteSpace(StreetAddress))
                addressParts.Add(StreetAddress);
            
            if (!string.IsNullOrWhiteSpace(City))
                addressParts.Add(City);
            
            if (!string.IsNullOrWhiteSpace(State))
                addressParts.Add(State);
            
            if (!string.IsNullOrWhiteSpace(ZipCode))
                addressParts.Add(ZipCode);
            
            if (!string.IsNullOrWhiteSpace(Country))
                addressParts.Add(Country);

            return string.Join(", ", addressParts);
        }
    }
}