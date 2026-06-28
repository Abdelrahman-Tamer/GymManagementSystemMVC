using GymManagementSystemMVC.DAL.Enum;
using GymManagementSystemMVC.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystemMVC.BLL.ViewModels.TrainerViewModels
{
    public class CreateTrainerViewModel
    {
        [Required]
        public string Name { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        [Phone]
        public string Phone { get; set; } = default!;

        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Specialites Specialites { get; set; }
        public int BuildingNumber { get; set; }
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
    }
}
