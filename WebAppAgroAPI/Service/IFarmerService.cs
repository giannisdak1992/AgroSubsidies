using WebAppAgroAPI.Data;
using WebAppAgroAPI.DTO;
using WebAppAgroAPI.Models;

namespace WebAppAgroAPI.Service
{
    public interface IFarmerService
    {
        Task<FarmerResponseDTO> GetFarmerByUserIdAsync(int userId);

        Task<FarmerResponseDTO> GetFarmerByTaxIdAsync(string taxId);

        Task<FarmerResponseDTO> GetFarmerWithUserAsync(int farmerId);

        Task<PaginatedResult<FarmerResponseDTO>> GetAllFarmersFilteredPaginatedAsync(int pageNumber, int pageSize, FarmerFiltersDTO farmerFiltersDTO);

        Task<FarmerResponseDTO> RegisterFarmerAsync(FarmerSignupDTO signupDTO);


    }
}
