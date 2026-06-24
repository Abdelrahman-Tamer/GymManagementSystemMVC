using GymManagementSystemMVC.DAL.Models;
using GymManagementSystemMVC.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemMVC.DAL.Repositories.Classes
    {
    public class MockeRepository : IPlanRepository
        {
        public Task<int> AddAsync( Plan plan )
            {
            throw new NotImplementedException();
            }

        public Task<int> DeleteAsync( Plan plan )
            {
            throw new NotImplementedException();
            }

        public async Task<IEnumerable<Plan>> GetAllAsync( bool tracking = false, CancellationToken ct = default )
            {
            List<Plan> plans = new List<Plan>()
                    {
                        new(){Name="Test"}
                    };
            return await Task.FromResult(plans.AsEnumerable());
            }

        public Task<Plan?> GetByIdAsync( int id, CancellationToken ct = default )
            {
            throw new NotImplementedException();
            }

        public Task<int> UbdateAsync( Plan plan )
            {
            throw new NotImplementedException();
            }
        }
    }
