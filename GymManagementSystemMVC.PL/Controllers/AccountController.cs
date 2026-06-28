using GymManagementSystemMVC.BLL.ViewModels.AccountViewModels;
using GymManagementSystemMVC.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystemMVC.PL.Controllers
{
    public class AccountController : Controller
    {
        #region Fields
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        #endregion

        #region Constructor
        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }
        #endregion

        #region Login
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction(nameof(HomeController.Index), "Home");

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null || string.IsNullOrWhiteSpace(user.UserName))
            {
                AddInvalidLoginError();
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {UserId} signed in", user.Id);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User {UserId} is locked out", user.Id);
                ModelState.AddModelError(string.Empty, "This Account Is Temporarily Locked Try Again Later");
                return View(model);
            }

            if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Sign In Not Allowed For This Account");
                return View(model);
            }

            AddInvalidLoginError();
            return View(model);
        }
        #endregion

        #region Logout
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region Access Denied
        [HttpGet]
        public IActionResult AccessDenied()
            => View();
        #endregion

        #region Helpers
        private void AddInvalidLoginError()
        {
            ModelState.AddModelError(string.Empty, "Invalid Email Or Password");
            ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
        }
        #endregion
    }
}
