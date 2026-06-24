using GymManagementSystemMVC.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymManagementSystemMVC.DAL.DbContexts
    {
    public class GYMDbContext : DbContext
        {
        public GYMDbContext(DbContextOptions<GYMDbContext> options) : base(options)
            {
                
            }
        #region DbSets
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<Plan> Plans { get; set; }
        #endregion
        protected override void OnModelCreating( ModelBuilder modelBuilder )
            {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            }
        }
    }