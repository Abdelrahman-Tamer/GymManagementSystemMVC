using GymManagementSystemMVC.DAL.DbContexts;
using GymManagementSystemMVC.DAL.Models;
using GymManagementSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemMVC.DAL.Repositories.Classes
    {
    public class PlanRepository : IPlanRepository
        {
        private readonly GYMDbContext _dbcontext;

        public PlanRepository( GYMDbContext dbcontext )
            {
            _dbcontext = dbcontext;
            }
        public async Task<int> AddAsync( Plan plan )
            {
            _dbcontext.Plans.Add(plan);
            return await _dbcontext.SaveChangesAsync();
            }

        public async Task<int> DeleteAsync( Plan plan )
            {
            _dbcontext.Plans.Remove(plan);
            return await _dbcontext.SaveChangesAsync();
            }

        public async Task<IEnumerable<Plan>> GetAllAsync( bool tracking = false, CancellationToken ct = default )
            {
            IQueryable<Plan> query = tracking? _dbcontext.Plans: _dbcontext.Plans.AsNoTracking();
            return await query.ToListAsync();
            }

        public async Task<Plan?> GetByIdAsync( int id, CancellationToken ct = default )
            {
            return await _dbcontext.Plans.FindAsync(id, ct);
            }

        public async Task<int> UbdateAsync( Plan plan )
            {
            _dbcontext.Plans.Update(plan);
            return await _dbcontext.SaveChangesAsync();
            }
        }
    }
