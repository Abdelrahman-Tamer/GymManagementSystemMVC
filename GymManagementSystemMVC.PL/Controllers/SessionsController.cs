using GymManagementSystemMVC.BLL.Services.Interfaces;
using GymManagementSystemMVC.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystemMVC.PL.Controllers
{
    public class SessionsController : Controller
    {
        #region Fields
        private readonly ISessionService _sessionService;
        #endregion

        #region Constructor
        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index(CancellationToken ct)
            => View(await _sessionService.GetAllSessionsAsync(ct));
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropdownAsync(ct);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownAsync(ct);
                return View(model);
            }

            var result = await _sessionService.CreateSessionAsync(model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.Error;
            await PopulateDropdownAsync(ct);
            return View(model);
        }
        #endregion

        #region Helpers
        private async Task PopulateDropdownAsync(CancellationToken ct)
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(ct), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoriesForDropDownAsync(ct), "Id", "CategoryName");
        }
        #endregion
    }
}
