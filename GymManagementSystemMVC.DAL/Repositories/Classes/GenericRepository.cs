using GymManagementSystemMVC.DAL.DbContexts;
using GymManagementSystemMVC.DAL.Models;
using GymManagementSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymManagementSystemMVC.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        #region Fields
        private readonly GYMDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        #endregion

        #region Constructor
        public GenericRepository(GYMDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }
        #endregion

        #region Queries
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _dbSet : _dbSet.AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _dbSet.FindAsync([id], ct);

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
            => _dbSet.AnyAsync(predicate, ct);

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool tracking, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _dbSet : _dbSet.AsNoTracking();
            return await query.FirstOrDefaultAsync(predicate, ct);
        }
        #endregion

        #region Commands
        public async Task<int> AddAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            return await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}
