using GymManagementSystemMVC.DAL.Models;

namespace GymManagementSystemMVC.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        #region Repositories
        ISessionRepository SessionRepository { get; }
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        #endregion

        #region Commands
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        #endregion
    }
}
