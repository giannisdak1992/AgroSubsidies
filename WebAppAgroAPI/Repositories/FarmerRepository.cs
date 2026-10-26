using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAppAgroAPI.Data;
using WebAppAgroAPI.Models;

namespace WebAppAgroAPI.Repositories
{
    public class FarmerRepository : BaseRepository<Farmer>, IFarmerRepository
    {
        public FarmerRepository(AgroSubsidiesAppDbContext context) : base(context)
        {
        }

        public async Task<Farmer?> GetFarmerByTaxIdAsync(string taxId)
        {
            return await _context.Farmers
               .Include(f => f.User)
               .FirstOrDefaultAsync(f => f.TaxIdentificationNumber == taxId);
        }

        
        public async Task<Farmer?> GetFarmerByUserIdAsync(int userId)
        {
            return await _context.Farmers.Include(f => f.User).FirstOrDefaultAsync(f => f.UserId == userId);
        }

        public async Task<Farmer?> GetFarmerWithUserAsync(int farmerId)
        {
            return await _context.Farmers.Include(f => f.User).FirstOrDefaultAsync(f => f.Id == farmerId);
        }

        public async Task<PaginatedResult<Farmer>> GetAllFarmersFilteredPaginatedAsync(int pageNumber,int pageSize, List<Expression<Func<Farmer, bool>>> predicates)
        {
            IQueryable<Farmer> query = _context.Farmers
                .Include(f => f.User);

            // filters
            if (predicates != null && predicates.Any())
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }
            // count. sql query
            var totalRecords = await query.CountAsync();

            // pagination
            int skip = (pageNumber - 1) * pageSize;

            // sql query
            var farmers = await query
                .OrderBy(f => f.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Farmer>
            {
                Data = farmers,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
