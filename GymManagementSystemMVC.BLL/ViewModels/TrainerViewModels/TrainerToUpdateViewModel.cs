using GymManagementSystemMVC.DAL.Enums;

namespace GymManagementSystemMVC.BLL.ViewModels.TrainerViewModels
{
    public class TrainerToUpdateViewModel
    {
        public string? Name { get; set; }
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public DateTime? DateOfBirth { get; set; }
        public Specialites Specialites { get; set; }
        public int BuildingNumber { get; set; }
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
    }
}
