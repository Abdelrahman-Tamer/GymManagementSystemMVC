using GymManagementSystemMVC.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GymManagementSystemMVC.DAL.DataSeeding
{
    public static class IdentityDataSeeding
    {
        private const string SeedPassword = "P@ssw0rd";
        private static readonly string[] Roles = { "SuperAdmin", "Admin" };

        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger logger, CancellationToken ct = default)
        {
            try
            {
                foreach (var roleName in Roles)
                {
                    if (await roleManager.RoleExistsAsync(roleName))
                        continue;

                    var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        logger.LogError("Failed To Create Role: {Role}. {Errors}", roleName, string.Join(";", roleResult.Errors.Select(e => e.Description)));
                    }
                }

                var mainAdmin = new ApplicationUser
                {
                    FirstName = "Rana",
                    LastName = "Hatem",
                    UserName = "RanaHatem",
                    Email = "RanaHatem@gmail.com",
                    PhoneNumber = "01225770196"
                };

                await CreateUserAsync(userManager, logger, mainAdmin, SeedPassword, "SuperAdmin", "Admin");

                var admin01 = new ApplicationUser
                {
                    FirstName = "Sara",
                    LastName = "Ahmed",
                    UserName = "SaraAhmed",
                    Email = "SaraAhmed@gmail.com",
                    PhoneNumber = "01225770196"
                };

                await CreateUserAsync(userManager, logger, admin01, SeedPassword, "Admin");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Identity Seeding Failed");
                throw;
            }
        }

        private static async Task CreateUserAsync(UserManager<ApplicationUser> userManager, ILogger logger, ApplicationUser user, string password, params string[] roles)
        {
            var existingUser = await userManager.FindByEmailAsync(user.Email!);
            if (existingUser is null)
            {
                var createResult = await userManager.CreateAsync(user, password);
                if (!createResult.Succeeded)
                {
                    logger.LogError("Failed To Create Seed User: {Email}. {Errors}", user.Email, string.Join(";", createResult.Errors.Select(e => e.Description)));
                    return;
                }

                existingUser = user;
                logger.LogInformation("Seeded user {Email}", user.Email);
            }

            foreach (var role in roles)
            {
                if (await userManager.IsInRoleAsync(existingUser, role))
                    continue;

                var addToRoleResult = await userManager.AddToRoleAsync(existingUser, role);
                if (!addToRoleResult.Succeeded)
                    logger.LogError("Failed To Add User {Email} To Role {Role}. {Errors}", existingUser.Email, role, string.Join(";", addToRoleResult.Errors.Select(e => e.Description)));
            }
        }
    }
}
