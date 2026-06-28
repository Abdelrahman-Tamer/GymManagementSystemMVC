using GymManagementSystemMVC.DAL.DbContexts;
using GymManagementSystemMVC.DAL.Models;
using GymManagementSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymManagementSystemMVC.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        #region Fields
        private readonly GYMDbContext _dbContext;
        #endregion

        #region Constructor
        public SessionRepository(GYMDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Queries
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(
            Expression<Func<Session, bool>>? predicate = null,
            CancellationToken ct = default)
        {
            IQueryable<Session> query = _dbContext.Sessions
                .AsNoTracking()
                .Include(s => s.Trainer)
                .Include(s => s.Category);

            if (predicate is not null)
                query = query.Where(predicate);

            return await query.ToListAsync(ct);
        }

        public Task<Session?> GetSessionWithTrainerAndCategoryAsync(int sessionId, CancellationToken ct = default)
            => _dbContext.Sessions
                .AsNoTracking()
                .Include(s => s.Trainer)
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == sessionId, ct);

        public Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default)
            => _dbContext.Bookings
                .AsNoTracking()
                .CountAsync(b => b.SessionId == sessionId, ct);
        #endregion
    }
}
