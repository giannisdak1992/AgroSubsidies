using System.Linq.Expressions;
using WebAppAgroAPI.Data;
using WebAppAgroAPI.Models;

namespace WebAppAgroAPI.Repositories
{
    public interface IFarmerRepository
    {
        Task<Farmer?> GetFarmerByUserIdAsync(int userId);

        Task<Farmer?> GetFarmerByTaxIdAsync(string taxId);

        Task<Farmer?> GetFarmerWithUserAsync(int farmerId);

        Task<PaginatedResult<Farmer>> GetAllFarmersFilteredPaginatedAsync(int pageNumber, int pageSize, List<Expression<Func<Farmer, bool>>> predicates);
    }
}
