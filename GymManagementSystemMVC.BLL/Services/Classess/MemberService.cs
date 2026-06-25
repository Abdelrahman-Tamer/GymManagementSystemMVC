using GymManagementSystemMVC.BLL.Services.Interfaces;
using GymManagementSystemMVC.BLL.ViewModels.MemberViewModels;
using GymManagementSystemMVC.DAL.Models;
using GymManagementSystemMVC.DAL.Repositories.Classes;
using GymManagementSystemMVC.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemMVC.BLL.Services.Classess
    {
    public class MemberService : IMemberService
        {
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<MemberShip> _memberShipRepository;
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IGenericRepository<HealthRecord> _healthRecordRepository;
        private readonly IGenericRepository<Booking> _bookingRepository;

        public MemberService( IGenericRepository<Member> MemberRepository, IGenericRepository<MemberShip> MemberShipRepository, IGenericRepository<Plan> PlanRepository, IGenericRepository<HealthRecord> HealthRecordRepository, IGenericRepository<Booking> BookingRepository )
            {
            _memberRepository = MemberRepository; 
            _memberShipRepository = MemberShipRepository; 
            _planRepository = PlanRepository;
            _healthRecordRepository = HealthRecordRepository;
            _bookingRepository = BookingRepository;

            }

        public async Task<bool> CreateMemberAsync( CreateMemberViewModel model, CancellationToken ct = default )
            {
            //Check Email Exists
            var emailExists = await _memberRepository.AnyAsync(x => x.Email == model.Email, ct);
            //Check Phone Exists
            var phoneExists = await _memberRepository.AnyAsync(x => x.Phone == model.Phone, ct);
            if ( emailExists || phoneExists ) return false;

            // Manual Mapping
            var member = new Member()
                {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Address = new Address()
                    {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                    },
                HealthRecord = new HealthRecord()
                    {
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Height = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight
                    }
                };

            var Result = await _memberRepository.AddAsync(member);
            return Result > 0;
            }

        public async Task<IEnumerable<MemberViewModel>> GetALLMembersAsync( CancellationToken ct = default )
            {
            var members = await _memberRepository.GetAllAsync(ct: ct);
            if ( !members.Any() ) return [];

            var membersViewModel = members.Select(m => new MemberViewModel
                {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo,
                Gender = m.Gender.ToString()
                });

            return membersViewModel;
            }

        public async Task<MemberViewModel?> GetMemberDetailsAsync( int memberId, CancellationToken ct = default )
            {
            var Member = await _memberRepository.GetByIdAsync(memberId, ct);
            if ( Member is null ) return null;

            var viewModel = new MemberViewModel
                {
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                DateOfBirth = Member.DateOfBirth.ToShortDateString(),
                Gender = Member.Gender.ToString(),
                Address = $"{Member.Address.BuildingNumber}-{Member.Address.Street}-{Member.Address.City}",
                };
            var ActiveMemberShip = await _memberShipRepository.FirstOrDefaultAsync(m => m.MemberId == memberId && m.EndDate > DateTime.UtcNow,tracking: false,ct: ct); 
            if ( ActiveMemberShip is not null )
                {
                var activePlan = await _planRepository.GetByIdAsync(ActiveMemberShip.PlanId, ct);
                viewModel.PlanName = activePlan?.Name;
                viewModel.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                viewModel.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
                }

            return viewModel;
            }

        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync( int memberid, CancellationToken ct = default )
            {
            var member = await _memberRepository.GetByIdAsync(memberid, ct);
            if ( member is null ) return null;
            else
                {
                return new MemberToUpdateViewModel()
                    {
                    Name = member.Name,
                    Email = member.Email,
                    Phone = member.Phone,
                    Gender = member.Gender.ToString(),
                    DateOfBirth = member.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                    Street = member.Address.Street,
                    City = member.Address.City,
                    BuildingNumber = member.Address.BuildingNumber,
                    Photo = member.Photo
                    };
                }
            }

        public async Task<bool> UpdateMemberDetailsAsync( int id, MemberToUpdateViewModel model, CancellationToken ct = default )
            {
            var member = await _memberRepository.GetByIdAsync(id, ct);
            if ( member is null ) return false;
            if ( await _memberRepository.AnyAsync(M => M.Email == model.Email && M.Id != id, ct) )
                return false;
            if ( await _memberRepository.AnyAsync(M => M.Phone == model.Phone && M.Id != id, ct) )
                return false;

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.City = model.City;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.Street = model.Street;
            member.UpdatedAt = DateTime.Now;

            var result = await _memberRepository.UpdateAsync(member);
            return result > 0;
            }

        public async Task<HealthRecordViewModel?> GetmemberHealthRecordAsync( int memberId, CancellationToken ct = default )
            {
            var record = await _healthRecordRepository.FirstOrDefaultAsync(x => x.MemberId == memberId, tracking: false, ct: ct);
            if ( record is null ) return null;
            else
                {
                return new HealthRecordViewModel()
                    {
                    Weight = record.Weight,
                    BloodType = record.BloodType,
                    Height = record.Height,
                    Note = record.Note
                    };
                }
            }

        public async Task<bool> RemoveMemberAsync( int id, CancellationToken ct = default )
            {
            var member = await _memberRepository.GetByIdAsync(id, ct);
            if ( member is null ) return false;

            var hasFutureSessions = await _bookingRepository.AnyAsync(
                b => b.MemberId == id && b.Session.StartDate > DateTime.Now, ct);

            if ( hasFutureSessions ) return false;

            var Result = await _memberRepository.DeleteAsync(member);
            return Result > 0;
            }
        }
    }
