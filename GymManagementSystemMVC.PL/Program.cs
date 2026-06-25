using GymManagementSystemMVC.BLL.Services.Classess;
using GymManagementSystemMVC.BLL.Services.Interfaces;
using GymManagementSystemMVC.DAL.DbContexts;
using GymManagementSystemMVC.DAL.Repositories.Classes;
using GymManagementSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystemMVC.PL
    {
    public class Program
        {
        public static void Main( string[] args )
            {
            var builder = WebApplication.CreateBuilder(args);

            // MVC services
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IMemberService, MemberService>();

            // Register the DbContext
            builder.Services.AddDbContext<GYMDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //   Register our repositories
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddScoped<IPlanRepository, PlanRepository>();
            //builder.Services.AddScoped<IMemberRepository, MemberRepository>();

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if ( !app.Environment.IsDevelopment() )
                {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
            }
        }
    }
