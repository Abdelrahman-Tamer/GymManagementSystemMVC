using GymManagementSystemMVC.DAL.Models;
using System.Linq.Expressions;

namespace GymManagementSystemMVC.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        #region Queries
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(Expression<Func<Session, bool>>? predicate = null, CancellationToken ct = default);
        Task<Session?> GetSessionWithTrainerAndCategoryAsync(int sessionId, CancellationToken ct = default);
        Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default);
        #endregion
    }
}
