using Serilog;
using System.Linq.Expressions;
using WebAppAgroAPI.Data;
using WebAppAgroAPI.DTO;
using WebAppAgroAPI.Models;
using WebAppAgroAPI.Repositories;
using WebAppAgroAPI.Security;

namespace WebAppAgroAPI.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserService> _logger = new LoggerFactory().AddSerilog().CreateLogger<UserService>();
      
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User?> VerifyAndGetUserAsync(UserLoginDTO credentials)
        {
           

            try
            {
                User? user;
                user = await _unitOfWork.UserRepository.GetByUsernameAsync(credentials.UserName!);

                if (user == null)
                {
                    return null;
                }

                bool isValidPassword = EncryptionUtil.IsValidPassword(credentials.Password!, user.Password!);
                if (!isValidPassword)
                {
                    return null;
                }
                _logger.LogInformation("User {Username} found and verified.", user.Username);

                return user;
                    
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while verifying user {Username}", credentials.UserName);
                throw;
            }

        }

        public async Task<PaginatedResult<User>> GetAllUsersFilteredPaginatedAsync(int pageNumber, int pageSize, UserFiltersDTO userFiltersDTO)
        {
            // if not filters then initialize it as an empty dto and no predicates
           if (userFiltersDTO == null)
            {
                userFiltersDTO = new UserFiltersDTO();
            }

            var predicates = new List<Expression<Func<User, bool>>>();

            if(!string.IsNullOrWhiteSpace(userFiltersDTO.UserName))
            {
                predicates.Add(u => u.Username.Contains(userFiltersDTO.UserName));
            }

            if(!string.IsNullOrWhiteSpace(userFiltersDTO.Email))
            {
                predicates.Add(u => u.Email.Contains(userFiltersDTO.Email));
            }

            if (userFiltersDTO.UserRole.HasValue)
            {
                predicates.Add(u => u.UserRole == userFiltersDTO.UserRole.Value);
            }

            return await _unitOfWork.UserRepository.GetAllUsersFilteredPaginatedAsync(pageNumber, pageSize, predicates);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {

            try
            {

                User? user = await _unitOfWork.UserRepository.GetByUsernameAsync(username);
                return user;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving user with username {Username}", username);
                throw;
            }
        }

       
    }
}
