using AutoMapper;
using GymManagementSystemMVC.BLL.ViewModels.SessionViewModels;
using GymManagementSystemMVC.BLL.ViewModels.TrainerViewModels;
using GymManagementSystemMVC.DAL.Models;

namespace GymManagementSystemMVC.BLL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            MapSession();
        }

        #region Session Mappings
        private void MapSession()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : string.Empty))
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer != null ? src.Trainer.Name : string.Empty))
                .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore());

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<UpdateSessionViewModel, Session>().ReverseMap();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();
        }
        #endregion

        #region Trainer Mappings
        private void MapTrainer()
        {
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }));

            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.Address,
                    opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));

            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City));

            CreateMap<TrainerToUpdateViewModel, Trainer>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.City = src.City;
                    dest.Address.Street = src.Street;
                    dest.UpdatedAt = DateTime.Now;
                });
        }
        #endregion
    }
}
