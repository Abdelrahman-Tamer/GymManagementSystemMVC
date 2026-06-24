using GymManagementSystemMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystemMVC.DbContexts
{
    public class GYMDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=.;Database=GYMDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        public DbSet<Plan> Plans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Configurations.PlanConfiguration());
        }
    }
}
