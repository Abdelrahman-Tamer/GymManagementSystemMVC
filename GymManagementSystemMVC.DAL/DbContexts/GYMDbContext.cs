using GymManagementSystemMVC.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GymManagementSystemMVC.DAL.DbContexts
{
    public class GYMDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Constructor
        public GYMDbContext(DbContextOptions<GYMDbContext> options) : base(options)
        {
        }
        #endregion

        #region DbSets
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Session> Sessions { get; set; }
        #endregion

        #region Model Configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<ApplicationUser>(entity =>
                {
                    entity.Property(x => x.FirstName)
                        .HasColumnType("varchar")
                        .HasMaxLength(50);

                    entity.Property(x => x.LastName)
                        .HasColumnType("varchar")
                        .HasMaxLength(50);
                });
        }
        #endregion
    }
}
