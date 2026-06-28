using GymManagementSystemMVC.BLL;
using GymManagementSystemMVC.BLL.Services.AttachmentService;
using GymManagementSystemMVC.BLL.Services.Classess;
using GymManagementSystemMVC.BLL.Services.Interfaces;
using GymManagementSystemMVC.DAL.DbContexts;
using GymManagementSystemMVC.DAL.Models;
using GymManagementSystemMVC.DAL.Repositories.Classes;
using GymManagementSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystemMVC.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region Builder
            var builder = WebApplication.CreateBuilder(args);
            #endregion

            #region Services
            builder.Services.AddControllersWithViews();
            RegisterDatabase(builder.Services, builder.Configuration);
            RegisterIdentity(builder.Services);
            RegisterApplicationServices(builder.Services);
            #endregion

            #region App
            var app = builder.Build();
            #endregion

            #region Middleware
            ConfigureMiddleware(app);
            #endregion

            #region Endpoints
            MapEndpoints(app);
            #endregion

            #region Database And Seed Data
            await app.MigrateAndSeedDataAsync();
            #endregion

            app.Run();
        }

        #region Service Registration
        private static void RegisterDatabase(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GYMDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        private static void RegisterIdentity(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
                {
                    config.User.RequireUniqueEmail = true;
                    config.Lockout.MaxFailedAccessAttempts = 5;
                    config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                })
                .AddEntityFrameworkStores<GYMDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });
        }

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IPlanRepository, PlanRepository>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddAutoMapper(config => config.AddProfile(new MappingProfiles()));
        }
        #endregion

        #region Pipeline
        private static void ConfigureMiddleware(WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
        }

        private static void MapEndpoints(WebApplication app)
        {
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();
        }
        #endregion
    }
}
