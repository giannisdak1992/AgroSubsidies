
using WebAppAgroAPI.Data;

namespace WebAppAgroAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AgroSubsidiesAppDbContext _context;

        public UnitOfWork(AgroSubsidiesAppDbContext context)
        {
            _context = context;
        }
        public UserRepository UserRepository => new(_context);

        public SubsidyApplicationRepository SubsidyApplicationRepository => new(_context);

        public ParcelRepository ParcelRepository => new(_context);

        public FarmerRepository FarmerRepository => new(_context);

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
