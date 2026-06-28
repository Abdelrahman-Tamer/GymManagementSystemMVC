using GymManagementSystemMVC.BLL.Services.AttachmentService;
using GymManagementSystemMVC.BLL.Services.Interfaces;
using GymManagementSystemMVC.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystemMVC.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MembersController : Controller
    {
        #region Fields
        private readonly IMemberService _memberService;
        private readonly IAttachmentService _attachmentService;
        #endregion

        #region Constructor
        public MembersController(IMemberService memberService, IAttachmentService attachmentService)
        {
            _memberService = memberService;
            _attachmentService = attachmentService;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetALLMembersAsync(ct);
            return View(members);
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
            => View();

        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);

            var result = await _memberService.CreateMemberAsync(model, ct);
            TempData[result ? "SuccessMessage" : "ErrorMessage"] = result
                ? "Member created successfully."
                : "Failed to create member.";

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var record = await _memberService.GetmemberHealthRecordAsync(id, ct);
            if (record is null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(record);
        }
        #endregion

        #region Media
        [HttpGet]
        public async Task<IActionResult> Picture(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsAsync(id, ct);
            if (member?.Photo is null) return NotFound();

            var file = _attachmentService.GetFile(member.Photo, "members");
            if (file is null) return NotFound();

            return File(file.Value.stream, file.Value.ContentType);
        }
        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberToUpdateAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> EditMember(int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _memberService.UpdateMemberDetailsAsync(id, model, ct);
            if (result)
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
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _memberService.RemoveMemberAsync(id, ct);
            TempData[result ? "SuccessMessage" : "ErrorMessage"] = result
                ? "Member deleted successfully."
                : "Failed to delete member";

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
