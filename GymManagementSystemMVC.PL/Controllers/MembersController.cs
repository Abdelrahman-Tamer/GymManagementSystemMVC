using GymManagementSystemMVC.BLL.Services.Interfaces;
using GymManagementSystemMVC.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystemMVC.PL.Controllers
    {
    public class MembersController : Controller
        {
        private readonly IMemberService _memberService;

        public MembersController( IMemberService memberService )
            {
            _memberService = memberService;
            }

        #region Index
        public async Task<IActionResult> Index( CancellationToken ct )
            {
            var members = await _memberService.GetALLMembersAsync(ct);
            return View(members);
            }
        #endregion
        #region Create
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateMember( CreateMemberViewModel model, CancellationToken ct )
            {
            if ( !ModelState.IsValid ) return View(nameof(Create), model);

            var result = await _memberService.CreateMemberAsync(model, ct);
            if ( result )
                TempData["SuccessMessage"] = "Member created successfully.";
            else
                TempData["ErrorMessage"] = "Failed to create member.";

            return RedirectToAction(nameof(Index));
            }
        #endregion
        #region MemberDetails
        [HttpGet]
        public async Task<IActionResult> MemberDetails( int id, CancellationToken ct )
            {
            var member = await _memberService.GetMemberDetailsAsync(id, ct);
            if ( member is null )
                {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
                }
            return View(member);
            }
        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails( int id, CancellationToken ct )
            {
            var record = await _memberService.GetmemberHealthRecordAsync(id, ct);
            if ( record is null )
                {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
                }
            return View(record);
            }
        #endregion
        #region Edit
        [HttpGet]
        public async Task<IActionResult> EditMember( int id, CancellationToken ct )
            {
            var member = await _memberService.GetMemberToUpdateAsync(id, ct);
            if ( member is null )
                {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
                }
            return View(member);
            }

        [HttpPost]
        public async Task<IActionResult> EditMember( int id, MemberToUpdateViewModel model, CancellationToken ct )
            {
            if ( !ModelState.IsValid ) return View(model);

            var result = await _memberService.UpdateMemberDetailsAsync(id, model, ct);
            if ( result )
                {
                TempData["SuccessMessage"] = "Member updated successfully.";
                return RedirectToAction(nameof(Index));
                }

            TempData["ErrorMessage"] = "Failed to update member.";
            return View(model);
            }

        #endregion
        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete( int id, CancellationToken ct )
            {
            var member = await _memberService.GetMemberDetailsAsync(id, ct);
            if ( member is null )
                {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
                }
            return View(member);
            }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed( int id, CancellationToken ct )
            {
            var result = await _memberService.RemoveMemberAsync(id, ct);
            if ( result )
                TempData["SuccessMessage"] = "Member deleted successfully.";
            else
                TempData["ErrorMessage"] = "Failed to delete member";

            return RedirectToAction(nameof(Index));
            }
        #endregion

        }
    }

