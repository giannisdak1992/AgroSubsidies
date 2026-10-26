using System.Linq.Expressions;
using WebAppAgroAPI.Data;
using WebAppAgroAPI.Models;

namespace WebAppAgroAPI.Repositories
{
    public interface IParcelRepository
    {
        Task<IEnumerable<Parcel>> GetParcelsByFarmerIdAsync(int farmerId);

        Task<Parcel?> GetParcelWithDetailsAsync(int parcelId);

        Task<IEnumerable<ParcelStatisticsResult>> GetParcelStatisticsResultsAsync();

        Task<PaginatedResult<Parcel>> GetAllParcelsFilteredPaginatedAsync(int pageNumber, int pageSize, List<Expression<Func<Parcel, bool>>> predicates);


    }
}
