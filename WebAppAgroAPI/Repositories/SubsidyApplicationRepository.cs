
using Microsoft.EntityFrameworkCore;
using WebAppAgroAPI.Core;
using WebAppAgroAPI.Data;

namespace WebAppAgroAPI.Repositories
{
    public class SubsidyApplicationRepository : BaseRepository<SubsidyApplication>, ISubsidyApplicationRepository
    {
        public SubsidyApplicationRepository(AgroSubsidiesAppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<SubsidyApplication>> GetApplicationsByFarmerIdAsync(int farmerId)
        {
            return await _context.SubsidyApplications.Where(sa => sa.FarmerId == farmerId).ToListAsync();

        }

        public async Task<IEnumerable<SubsidyApplication>> GetApplicationsBySubsidyTypeAsync(SubsidyType subsidyType)
        {
            return await _context.SubsidyApplications.Where(sa => sa.SubsidyType == subsidyType).ToListAsync();
        }

        public async Task<SubsidyApplication?> GetApplicationWithDetailsAsync(int applicationId)
        {
            return await _context.SubsidyApplications
                .Include(sa => sa.Farmer)
                    .ThenInclude(f => f.User)
                .Include(sa => sa.ApplicationParcels)
                    .ThenInclude(ap => ap.Parcel)
                        .ThenInclude(p => p.RegionalUnit)
                .FirstOrDefaultAsync(sa => sa.Id == applicationId);

        }
       
    }
}
