using GymManagementSystemMVC.DAL.DbContexts;
using GymManagementSystemMVC.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GymManagementSystemMVC.DAL.DataSeeding
{
    public static class GymDataSeeding
    {
        public static async Task SeedAsync(GYMDbContext dbContext, string seedFilePath, ILogger logger, CancellationToken ct = default)
        {
            try
            {
                if (!await dbContext.Plans.AnyAsync(ct))
                {
                    var Plans = LoadDataFromJsonFile<Plan>("plans.json", seedFilePath);
                    if (Plans.Count > 0)
                    {
                        dbContext.Plans.AddRange(Plans);
                        logger.LogInformation("Seeded {Count} plans", Plans.Count);
                    }
                }

                if (dbContext.ChangeTracker.HasChanges())
                    await dbContext.SaveChangesAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gym Data Seeding Failed");
                throw;
            }
        }

        private static List<T> LoadDataFromJsonFile<T>(string fileName, string folderPath)
        {
            var filePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Seed Data File Not Found: {filePath}");

            var data = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new JsonStringEnumConverter());
            return JsonSerializer.Deserialize<List<T>>(data, options) ?? [];
        }
    }
}
