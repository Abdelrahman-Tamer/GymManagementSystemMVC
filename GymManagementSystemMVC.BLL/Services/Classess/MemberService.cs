using GymManagementSystemMVC.BLL.Services.AttachmentService;
using GymManagementSystemMVC.BLL.Services.Interfaces;
using GymManagementSystemMVC.BLL.ViewModels.MemberViewModels;
using GymManagementSystemMVC.DAL.Models;
using GymManagementSystemMVC.DAL.Repositories.Interfaces;

namespace GymManagementSystemMVC.BLL.Services.Classess
{
    public class MemberService : IMemberService
    {
        #region Fields
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<MemberShip> _memberShipRepository;
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IGenericRepository<HealthRecord> _healthRecordRepository;
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IAttachmentService _attachmentService;
        #endregion

        #region Constructor
        public MemberService(
            IGenericRepository<Member> memberRepository,
            IGenericRepository<MemberShip> memberShipRepository,
            IGenericRepository<Plan> planRepository,
            IGenericRepository<HealthRecord> healthRecordRepository,
            IGenericRepository<Booking> bookingRepository,
            IAttachmentService attachmentService)
        {
            _memberRepository = memberRepository;
            _memberShipRepository = memberShipRepository;
            _planRepository = planRepository;
            _healthRecordRepository = healthRecordRepository;
            _bookingRepository = bookingRepository;
            _attachmentService = attachmentService;
        }
        #endregion

        #region Queries
        public async Task<IEnumerable<MemberViewModel>> GetALLMembersAsync(CancellationToken ct = default)
        {
            var members = await _memberRepository.GetAllAsync(ct: ct);
            if (!members.Any()) return [];

            return members.Select(member => new MemberViewModel
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString()
            });
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _memberRepository.GetByIdAsync(memberId, ct);
            if (member is null) return null;

            var viewModel = new MemberViewModel
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address = $"{member.Address.BuildingNumber}-{member.Address.Street}-{member.Address.City}",
            };

            var activeMemberShip = await _memberShipRepository.FirstOrDefaultAsync(
                m => m.MemberId == memberId && m.EndDate > DateTime.UtcNow,
                tracking: false,
                ct: ct);

            if (activeMemberShip is not null)
            {
                var activePlan = await _planRepository.GetByIdAsync(activeMemberShip.PlanId, ct);
                viewModel.PlanName = activePlan?.Name;
                viewModel.MemberShipStartDate = activeMemberShip.StartDate.ToShortDateString();
                viewModel.MemberShipEndDate = activeMemberShip.EndDate.ToShortDateString();
            }

            return viewModel;
        }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _memberRepository.GetByIdAsync(memberId, ct);
            if (member is null) return null;

            return new MemberToUpdateViewModel
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                BuildingNumber = member.Address.BuildingNumber,
                City = member.Address.City,
                Street = member.Address.Street,
                Photo = member.Photo
            };
        }

        public async Task<HealthRecordViewModel?> GetmemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var record = await _healthRecordRepository.FirstOrDefaultAsync(x => x.MemberId == memberId, tracking: false, ct: ct);
            if (record is null) return null;

            return new HealthRecordViewModel
            {
                Weight = record.Weight,
                BloodType = record.BloodType,
                Height = record.Height,
                Note = record.Note
            };
        }
        #endregion

        #region Commands
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var emailExists = await _memberRepository.AnyAsync(x => x.Email == model.Email, ct);
            var phoneExists = await _memberRepository.AnyAsync(x => x.Phone == model.Phone, ct);
            if (emailExists || phoneExists) return false;

            var photoName = await _attachmentService.UploadAsync(model.Photo, "members", ct);
            if (model.Photo is not null && photoName is null) return false;

            var member = new Member
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Photo = photoName,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                },
                HealthRecord = new HealthRecord
                {
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Height = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight
                }
            };

            var affectedRows = await _memberRepository.AddAsync(member);
            if (affectedRows <= 0 && photoName is not null)
                _attachmentService.Delete(photoName, "members");

            return affectedRows > 0;
        }

        public async Task<bool> UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var member = await _memberRepository.GetByIdAsync(id, ct);
            if (member is null) return false;

            if (await _memberRepository.AnyAsync(m => m.Email == model.Email && m.Id != id, ct))
                return false;

            if (await _memberRepository.AnyAsync(m => m.Phone == model.Phone && m.Id != id, ct))
                return false;

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.City = model.City;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.Street = model.Street;
            member.UpdatedAt = DateTime.Now;

            var affectedRows = await _memberRepository.UpdateAsync(member);
            return affectedRows > 0;
        }

        public async Task<bool> RemoveMemberAsync(int id, CancellationToken ct = default)
        {
            var member = await _memberRepository.GetByIdAsync(id, ct);
            if (member is null) return false;

            var hasFutureSessions = await _bookingRepository.AnyAsync(
                b => b.MemberId == id && b.Session.StartDate > DateTime.Now,
                ct);

            if (hasFutureSessions) return false;

            var affectedRows = await _memberRepository.DeleteAsync(member);
            if (affectedRows > 0 && member.Photo is not null)
                _attachmentService.Delete(member.Photo, "members");

            return affectedRows > 0;
        }
        #endregion
    }
}
