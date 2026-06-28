using GymManagementSystemMVC.DAL.DbContexts;
using GymManagementSystemMVC.DAL.Models;
using GymManagementSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystemMVC.DAL.Repositories.Classes
{
    public class PlanRepository : IPlanRepository
    {
        #region Fields
        private readonly GYMDbContext _dbContext;
        #endregion

        #region Constructor
        public PlanRepository(GYMDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Queries
        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<Plan> query = tracking ? _dbContext.Plans : _dbContext.Plans.AsNoTracking();
            return await query.ToListAsync(ct);
        }

        public async Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _dbContext.Plans.FindAsync([id], ct);
        #endregion

        #region Commands
        public async Task<int> AddAsync(Plan plan)
        {
            _dbContext.Plans.Add(plan);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UbdateAsync(Plan plan)
        {
            _dbContext.Plans.Update(plan);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Plan plan)
        {
            _dbContext.Plans.Remove(plan);
            return await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}
