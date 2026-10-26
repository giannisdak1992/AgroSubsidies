using WebAppAgroAPI.Core;
using WebAppAgroAPI.Data;

namespace WebAppAgroAPI.Repositories
{
    public interface ISubsidyApplicationRepository
    {
        Task<IEnumerable<SubsidyApplication>> GetApplicationsByFarmerIdAsync(int farmerId);

        Task<IEnumerable<SubsidyApplication>> GetApplicationsBySubsidyTypeAsync(SubsidyType subsidyType);

        Task<SubsidyApplication?> GetApplicationWithDetailsAsync(int applicationId);
    }
}
