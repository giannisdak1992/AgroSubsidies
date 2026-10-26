using WebAppAgroAPI.Core;

namespace WebAppAgroAPI.DTO
{
    public class UserFiltersDTO
    {
        public string? UserName { get; set; }

        public string? Email { get; set; }


        public UserRole? UserRole { get; set; }
    }
}
