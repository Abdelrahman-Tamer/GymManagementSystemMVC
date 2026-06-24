using GymManagementSystemMVC.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystemMVC.Controllers
{
    public class PlansController(GYMDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var plans = await context.Plans.ToListAsync();
            return View(plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await context.Plans.FirstOrDefaultAsync(x => x.Id == id);

            if (plan == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }
    }
}
