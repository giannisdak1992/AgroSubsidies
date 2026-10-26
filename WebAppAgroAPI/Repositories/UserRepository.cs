using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAppAgroAPI.Data;
using WebAppAgroAPI.Models;
using WebAppAgroAPI.Security;

namespace WebAppAgroAPI.Repositories
{
    public class UserRepository : BaseRepository<User> , IUserRepository
    {
        public UserRepository(AgroSubsidiesAppDbContext context) : base(context)
        {
        }
        public async Task<PaginatedResult<User>> GetAllUsersFilteredPaginatedAsync(int pageNumber, int pageSize, List<Expression<Func<User, bool>>> predicates)
        {
            int skip = (pageNumber - 1) * pageSize;

            IQueryable<User> query = _context.Users;

            if (predicates != null && predicates.Any())
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            int totalRecords = await query.CountAsync();

            var users = await query
                .OrderBy(u => u.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<User>
            {
                Data = users,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        public virtual async Task<User?> GetByUsernameAsync(string username)
            => await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        


        public virtual async Task<User?> UpdateUserAsync(int id, User user)
        {

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (existingUser is null) return null; // Ensure incoming user exists
            if (existingUser.Id != id) return null; // Ensure incoming ID matches the existing user ID

            _context.Users.Attach(user);
            _context.Entry(user).State = EntityState.Modified;
            return existingUser;


        }

        internal async Task<PaginatedResult<Farmer>> GetAllUsersFilteredPaginatedAsync(int pageNumber, int pageSize, List<Expression<Func<Farmer, bool>>> predicates)
        {
            throw new NotImplementedException();
        }
    }
}
