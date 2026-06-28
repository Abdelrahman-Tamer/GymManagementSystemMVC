using GymManagementSystemMVC.DAL.Models;
using System.Linq.Expressions;

namespace GymManagementSystemMVC.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        #region Queries
        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool tracking, CancellationToken ct = default);
        #endregion

        #region Commands
        Task<int> AddAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);
        #endregion
    }
}
