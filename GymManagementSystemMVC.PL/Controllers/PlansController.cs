using GymManagementSystemMVC.DAL.DbContexts;
using GymManagementSystemMVC.DAL.Repositories.Classes;
using GymManagementSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystemMVC.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanRepository _planRepository;
        public PlansController(IPlanRepository planRepository )
            {
                _planRepository = planRepository;
            }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planRepository.GetAllAsync(ct : ct);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id ,CancellationToken ct)
        {
            var plan = await _planRepository.GetByIdAsync(id,ct);
            if(plan == null )
                {
                return RedirectToAction(nameof(Index));
                }
            return View(plan);
        }
    }
}
