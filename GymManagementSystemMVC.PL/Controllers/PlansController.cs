using GymManagementSystemMVC.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystemMVC.PL.Controllers
{
    public class PlansController : Controller
    {
        #region Fields
        private readonly IPlanRepository _planRepository;
        #endregion

        #region Constructor
        public PlansController(IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planRepository.GetAllAsync(ct: ct);
            return View(plans);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct);
            if (plan is null)
                return RedirectToAction(nameof(Index));

            return View(plan);
        }
        #endregion
    }
}
