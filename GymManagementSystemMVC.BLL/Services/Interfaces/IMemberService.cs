using GymManagementSystemMVC.BLL.ViewModels.MemberViewModels;

namespace GymManagementSystemMVC.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        #region Queries
        Task<IEnumerable<MemberViewModel>> GetALLMembersAsync(CancellationToken ct = default);
        Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default);
        Task<HealthRecordViewModel?> GetmemberHealthRecordAsync(int memberId, CancellationToken ct = default);
        #endregion

        #region Commands
        Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default);
        Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default);
        Task<bool> RemoveMemberAsync(int id, CancellationToken ct = default);
        #endregion
    }
}
