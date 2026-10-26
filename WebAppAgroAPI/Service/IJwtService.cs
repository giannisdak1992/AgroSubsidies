using WebAppAgroAPI.Data;

namespace WebAppAgroAPI.Service
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
