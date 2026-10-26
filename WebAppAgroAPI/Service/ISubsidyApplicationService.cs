using WebAppAgroAPI.Data;
using WebAppAgroAPI.DTO;

namespace WebAppAgroAPI.Service
{
    public interface ISubsidyApplicationService
    {
        Task<SubsidyApplicationResponseDTO> SubmitApplicationAsync(int userId, SubsidyApplicationInsertDTO dto);

        Task<IEnumerable<SubsidyApplicationResponseDTO>> GetApplicationsByFarmerIdAsync(int farmerId);

        Task<IEnumerable<SubsidyApplicationResponseDTO>> GetMyApplicationsAsync(int userId);

        Task<SubsidyApplicationResponseDTO> EvaluateApplicationAsync(int applicationId);
    }
}
