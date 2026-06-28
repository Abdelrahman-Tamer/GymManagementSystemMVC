using GymManagementSystemMVC.DAL.DataSeeding;
using GymManagementSystemMVC.DAL.DbContexts;
using GymManagementSystemMVC.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystemMVC.PL
{
    public static class ProgramExtensions
    {
        public static async Task MigrateAndSeedDataAsync(this WebApplication app)
        {
            #region Service Scope
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GYMDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            #endregion

            #region Database Migrations
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                logger.LogInformation("Applying {Count} pending migrations", pendingMigrations.Count());
                await dbContext.Database.MigrateAsync();
            }
            #endregion

            #region Seed Data
            var seedFolderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeeding.SeedAsync(dbContext, seedFolderPath, logger);
            await IdentityDataSeeding.SeedAsync(roleManager, userManager, logger);
            #endregion
        }
    }
}
