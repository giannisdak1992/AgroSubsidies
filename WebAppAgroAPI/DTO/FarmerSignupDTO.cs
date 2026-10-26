using System.ComponentModel.DataAnnotations;

namespace WebAppAgroAPI.DTO
{
    public class FarmerSignupDTO
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 50 characters")]

        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W).{8,}$",
    ErrorMessage = "Password must contain at least one uppercase, one lowercase, one digit, and one special character")]

        public string? Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, ErrorMessage = "Email must not exceed 100 characters")]
        [EmailAddress(ErrorMessage = "Invalid email Address")]

        public string? Email {get ; set; }

        [Required(ErrorMessage = "Firstname is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Firstname must be between 2 and 50 characters")]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "Lastname is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Lastname must be between 2 and 50 characters")]

        public string? Lastname { get; set; }

        [Required(ErrorMessage = "Tax Identification Number is required")]
        [RegularExpression(
    @"^\d{9}$",
    ErrorMessage = "Tax Identification Number must contain exactly 9 digits")]
        public string? TaxIdentificationNumber { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateOnly? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be at least 10 characters and not exceed 15")]

        public string? PhoneNumber { get; set; }
    }
}
