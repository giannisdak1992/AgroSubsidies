using WebAppAgroAPI.Data;
using WebAppAgroAPI.DTO;
using WebAppAgroAPI.Models;

namespace WebAppAgroAPI.Service
{
    public interface IUserService
    {
        Task<User?> VerifyAndGetUserAsync(UserLoginDTO credentials);

        Task<User?> GetUserByUsernameAsync(string username);

        Task<PaginatedResult<User>> GetAllUsersFilteredPaginatedAsync(int pageNumber, int pageSize, UserFiltersDTO userFiltersDTO);
    }
}
