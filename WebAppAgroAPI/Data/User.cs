using System.Diagnostics.Eventing.Reader;
using WebAppAgroAPI.Core;

namespace WebAppAgroAPI.Data
{
    public class User : BaseEntity
    {
            public int Id { get; set; }
            public string Username { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
            public string Firstname { get; set; } = null!;
            public string Lastname { get; set; } = null!;
            public UserRole UserRole { get; set; }
        
            public bool IsActive { get; set; }

            /// <summary>
            /// The user's current account status.
            /// New users are registered with a pending status by default.
            /// </summary>
            public UserStatus UserStatus { get; set; } = UserStatus.Pending;
            public virtual Farmer? Farmer { get; set; }

            public virtual Employee? Employee { get; set; }

    }
}
