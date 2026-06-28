using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemMVC.BLL.ViewModels.MemberViewModels
{
    public class MemberToUpdateViewModel
    {
        public string? Name { get; set; }
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int BuildingNumber { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string? Photo { get; set; }
    }
}
