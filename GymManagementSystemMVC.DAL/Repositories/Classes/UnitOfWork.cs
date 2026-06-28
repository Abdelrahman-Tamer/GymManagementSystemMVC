using GymManagementSystemMVC.DAL.DbContexts;
using GymManagementSystemMVC.DAL.Models;
using GymManagementSystemMVC.DAL.Repositories.Interfaces;

namespace GymManagementSystemMVC.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields
        private readonly GYMDbContext _dbContext;
        private readonly Dictionary<string, object> _repositories = new();
        #endregion

        #region Constructor
        public UnitOfWork(GYMDbContext dbContext, ISessionRepository sessionRepository)
        {
            _dbContext = dbContext;
            SessionRepository = sessionRepository;
        }
        #endregion

        #region Repositories
        public ISessionRepository SessionRepository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var typeName = typeof(TEntity).Name;
            if (_repositories.TryGetValue(typeName, out var repository))
                return (IGenericRepository<TEntity>)repository;

            var genericRepository = new GenericRepository<TEntity>(_dbContext);
            _repositories[typeName] = genericRepository;
            return genericRepository;
        }
        #endregion

        #region Commands
        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _dbContext.SaveChangesAsync(ct);
        #endregion
    }
}
