using System.ComponentModel.DataAnnotations;

namespace WebAppAgroAPI.DTO
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 50 characters")]
        public string? UserName { get; set; }

        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*\W).{8,}$",
    ErrorMessage = "Password must contain at least one uppercase, one lowercase, one digit, and one special character")]

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

    }
}
