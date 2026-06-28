using AutoMapper;
using GymManagementSystemMVC.BLL.Common;
using GymManagementSystemMVC.BLL.Services.Interfaces;
using GymManagementSystemMVC.BLL.ViewModels.SessionViewModels;
using GymManagementSystemMVC.DAL.Enums;
using GymManagementSystemMVC.DAL.Models;
using GymManagementSystemMVC.DAL.Repositories.Interfaces;

namespace GymManagementSystemMVC.BLL.Services.Classess
{
    public class SessionService : ISessionService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Queries
        public async Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct: ct);
            if (sessions?.Any() != true) return null;

            sessions = sessions.OrderByDescending(x => x.StartDate);
            var mappedSessions = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity -
                    await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            }

            return mappedSessions;
        }

        public async Task<SessionViewModel?> GetSessionByIdAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategoryAsync(sessionId, ct);
            if (session is null) return null;

            var mappedSession = _mapper.Map<SessionViewModel>(session);
            mappedSession.AvailableSlots = mappedSession.Capacity -
                await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);

            return mappedSession;
        }

        public async Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(sessionId, ct);
            if (session is null) return null;
            if (!await IsSessionAvaliableForUpdatingAsync(session, ct)) return null;

            return _mapper.Map<UpdateSessionViewModel>(session);
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainerForDropDownAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoryForDropDownAsync(CancellationToken ct = default)
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default)
            => await GetTrainerForDropDownAsync(ct);

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default)
            => await GetCategoryForDropDownAsync(ct);
        #endregion

        #region Commands
        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate)
                return Result.Validation("End Date Must Be After Start Date");

            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start Date Must Be In The Future");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
            if (trainer is null)
                return Result.NotFound("Invalid Trainer Id");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId, ct);
            if (category is null)
                return Result.NotFound("Category Not Found");

            var isValidSpecialty = Enum.TryParse<Specialites>(category.CategoryName, true, out var categorySpecialty);
            if (!isValidSpecialty || trainer.Specialites != categorySpecialty)
                return Result.Validation("Trainer Specialty Does Not Match Session Category");

            var session = _mapper.Map<Session>(model);
            var affectedRows = await _unitOfWork.GetRepository<Session>().AddAsync(session);
            return affectedRows > 0 ? Result.Ok() : Result.Fail("Failed To Create Session");
        }

        public async Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var sessionRepo = _unitOfWork.GetRepository<Session>();
            var session = await sessionRepo.GetByIdAsync(id, ct);

            if (session is null)
                return Result.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot Update Session That Has Already Started");

            var bookedCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id, ct);
            if (bookedCount > 0)
                return Result.Fail("Can Not Update A Session That Has Bookings");

            if (model.EndDate <= model.StartDate)
                return Result.Validation("End Date Must Be After Start Date");

            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start Date Must Be In The Future");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
            if (trainer is null)
                return Result.NotFound("Invalid Trainer Id");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(session.CategoryId, ct);
            if (category is null)
                return Result.NotFound("Category Not Found");

            var isValidSpecialty = Enum.TryParse<Specialites>(category.CategoryName, true, out var categorySpecialty);
            if (!isValidSpecialty || trainer.Specialites != categorySpecialty)
                return Result.Validation("Trainer Specialty Does Not Match Session Category");

            _mapper.Map(model, session);
            session.UpdatedAt = DateTime.Now;

            var affectedRows = await sessionRepo.UpdateAsync(session);
            return affectedRows > 0 ? Result.Ok() : Result.Fail("Failed To Update Session");
        }

        public async Task<Result> RemoveSessionAsync(int sessionId, CancellationToken ct = default)
        {
            var sessionRepo = _unitOfWork.GetRepository<Session>();
            var session = await sessionRepo.GetByIdAsync(sessionId, ct);

            if (session is null)
                return Result.NotFound("Session Not Found");

            if (session.EndDate >= DateTime.Now)
                return Result.Fail("Can Not Delete A Session That Has Not Yet Ended");

            var bookedCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            if (bookedCount > 0)
                return Result.Fail("Can Not Delete A Session That Has Bookings");

            var affectedRows = await sessionRepo.DeleteAsync(session);
            return affectedRows > 0 ? Result.Ok() : Result.Fail("Failed To Delete Session");
        }
        #endregion

        #region Helper Methods
        private async Task<bool> IsSessionAvaliableForUpdatingAsync(Session session, CancellationToken ct = default)
        {
            if (session.StartDate <= DateTime.Now) return false;

            var booked = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            return booked == 0;
        }
        #endregion
    }
}
