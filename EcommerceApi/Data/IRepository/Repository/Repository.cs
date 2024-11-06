using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EcommerceApi.Data.IRepository.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dbset;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbset = _context.Set<TEntity>();
        }
        public async Task AddAsync(TEntity entity)
        {
             await _dbset.AddAsync(entity);
            await  _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbset.FindAsync(id);
            if (entity != null)
            {
                _dbset.Remove(entity);
              await  _context.SaveChangesAsync();
            }
        }

      

        public async Task<IEnumerable<TEntity>> DeleteAysncAll(Expression<Func<TEntity, bool>> predicate)
        {
            var list = await _context.Set<TEntity>()
                                 .Where(predicate)
                                 .ToListAsync();
            if (list.Any())
            {
                _context.Set<TEntity>().RemoveRange(list);
                await _context.SaveChangesAsync();
            }
            return list;
        }

        public async Task<IEnumerable<TEntity>> FilterListById(Expression<Func<TEntity, bool>> predicate)
        {
            var list = await _context.Set<TEntity>()
                           .Where(predicate)
                           .ToListAsync();
            return list;
        }

        public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbset.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            
            return await _dbset.FindAsync(id);
           
        }

        public async Task UpdateAsync(TEntity entity)
        {
           _dbset.Update(entity);
            await _context.SaveChangesAsync();

        }
    }
}
