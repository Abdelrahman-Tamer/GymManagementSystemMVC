using GymManagementSystemMVC.DAL.Models;

namespace GymManagementSystemMVC.DAL.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        #region Queries
        Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
        Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default);
        #endregion

        #region Commands
        Task<int> AddAsync(Plan plan);
        Task<int> UbdateAsync(Plan plan);
        Task<int> DeleteAsync(Plan plan);
        #endregion
    }
}
