using WebAppAgroAPI.Data;
using WebAppAgroAPI.DTO;
using WebAppAgroAPI.Models;

namespace WebAppAgroAPI.Service
{
    public interface IParcelService
    {
        Task<ParcelResponseDTO> AddParcelAsync(int userId, ParcelInsertDTO parcelInsertDTO);

        Task<IEnumerable<ParcelResponseDTO>> GetParcelsByFarmerIdAsync(int farmerId);

        Task<ParcelResponseDTO> GetParcelWithDetailsAsync(int parcelId);

        Task<ParcelResponseDTO> GetMyParcelWithDetailsAsync(int userId, int parcelId);

        Task<PaginatedResult<ParcelResponseDTO>> GetAllParcelsFilteredPaginatedAsync(int pageNumber, int pageSize, ParcelFiltersDTO parcelFiltersDTO);

        Task<IEnumerable<ParcelStatisticsDTO>> GetParcelStatisticsAsync();

        Task<IEnumerable<ParcelResponseDTO>> GetMyParcelsAsync(int userId);
    }
}
