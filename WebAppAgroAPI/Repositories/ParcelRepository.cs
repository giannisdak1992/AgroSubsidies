using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using WebAppAgroAPI.Data;
using WebAppAgroAPI.Models;

namespace WebAppAgroAPI.Repositories
{
    public class ParcelRepository : BaseRepository<Parcel>, IParcelRepository
    {
        public ParcelRepository(AgroSubsidiesAppDbContext context) : base(context)
        {
        }

        public async Task<PaginatedResult<Parcel>> GetAllParcelsFilteredPaginatedAsync(int pageNumber, int pageSize, List<Expression<Func<Parcel, bool>>> predicates)
        {
            IQueryable<Parcel> query = _context.Parcels
                   .Include(p => p.RegionalUnit);

            // filters
            if (predicates != null && predicates.Any())
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }
            // count
            var totalRecords = await query.CountAsync();

            // pagination
            int skip = (pageNumber - 1) * pageSize;

            var parcels = await query
                .OrderBy(p => p.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Parcel>
            {
                Data = parcels,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<Parcel>> GetParcelsByFarmerIdAsync(int farmerId)
        {
            return await _context.Parcels.Include(p => p.RegionalUnit).Where(p => farmerId == p.FarmerId).ToListAsync();
        }

        public async Task<IEnumerable<ParcelStatisticsResult>> GetParcelStatisticsResultsAsync()
        {
            return await _context.Parcels
                .GroupBy(p => new
                {
                    p.RegionalUnitId,
                    p.RegionalUnit.Name,
                    p.CropType
                })
                .Select(g => new ParcelStatisticsResult
                {
                    RegionalUnitId = g.Key.RegionalUnitId,
                    RegionalUnitName = g.Key.Name,
                    CropType = g.Key.CropType,
                    ParcelCount = g.Count(),
                    TotalArea = g.Sum(p => p.Area)
                })
                .OrderBy(r => r.RegionalUnitName)
                .ThenBy(r => r.CropType)
                .ToListAsync();
        }

        public async Task<Parcel?> GetParcelWithDetailsAsync(int parcelId)
        {
            return await _context.Parcels.Include(p => p.RegionalUnit)
                 .Include(p => p.Farmer)
                 .FirstOrDefaultAsync(p => p.Id == parcelId);
        }
    }
}
