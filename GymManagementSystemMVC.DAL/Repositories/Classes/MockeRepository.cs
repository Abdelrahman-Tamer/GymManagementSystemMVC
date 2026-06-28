using GymManagementSystemMVC.DAL.Models;
using GymManagementSystemMVC.DAL.Repositories.Interfaces;

namespace GymManagementSystemMVC.DAL.Repositories.Classes
{
    public class MockeRepository : IPlanRepository
    {
        #region Queries
        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            List<Plan> plans =
            [
                new() { Name = "Test" }
            ];

            return await Task.FromResult(plans.AsEnumerable());
        }

        public Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Commands
        public Task<int> AddAsync(Plan plan)
        {
            throw new NotImplementedException();
        }

        public Task<int> UbdateAsync(Plan plan)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Plan plan)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
