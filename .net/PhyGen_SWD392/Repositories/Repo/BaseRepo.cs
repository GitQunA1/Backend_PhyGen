using Microsoft.EntityFrameworkCore;
using PhyGen_SWD392.Models;
using PhyGen_SWD392.Repositories.Interface;

namespace PhyGen_SWD392.Repositories.Repo
{
    public class BaseRepo<T, TKey> : IBaseRepo<T, TKey> where T : class
    {
        private readonly PhyGenDbContext _context;
        public BaseRepo(PhyGenDbContext phyGenDbContext)
        {
            _context = phyGenDbContext;
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> UpdateAsync(TKey id, T entity)
        {
            var existing = await _context.Set<T>().FindAsync(id);

            if (existing == null)
            {
                return false;
            }

            _context.Entry(existing).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> Delete(TKey id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<T?> GetByIdAsync(TKey id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}
