using System.Linq.Expressions;
using WebAppAgroAPI.Data;
using WebAppAgroAPI.Models;

namespace WebAppAgroAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User?> UpdateUserAsync(int id, User user);

        Task<User?> GetByUsernameAsync(string username);

        Task<PaginatedResult<User>> GetAllUsersFilteredPaginatedAsync(int pageNumber, int pageSize,
            List<Expression<Func<User, bool>>> predicates);
    }
}
