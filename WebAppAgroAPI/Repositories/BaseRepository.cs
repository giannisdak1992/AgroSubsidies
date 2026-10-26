
using Microsoft.EntityFrameworkCore;
using WebAppAgroAPI.Data;

namespace WebAppAgroAPI.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public readonly AgroSubsidiesAppDbContext _context;
        private readonly DbSet<T> _dbSet;

        protected BaseRepository(AgroSubsidiesAppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
            => await _dbSet.AddRangeAsync(entities);


        public virtual async Task<bool> DeleteAsync(int id)
        {
           T? existingEntity = await GetAsync(id);
            if (existingEntity == null) return false;
            _dbSet?.Remove(existingEntity);
            return true;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
           => await _dbSet.ToListAsync();
       

        public virtual async Task<T?> GetAsync(int id)
            => await _dbSet.FindAsync(id);
        

        public virtual Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public virtual async Task<int> GetCountAsync()
            => await _dbSet.CountAsync();

    }
}
