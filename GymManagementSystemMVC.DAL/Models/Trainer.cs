using GymManagementSystemMVC.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemMVC.DAL.Models
{
    public class Trainer : GymUser
    {
        public Specialites Specialites { get; set; }
        public DateTime HireDate { get; set; }
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
        public ICollection<Session> TrainerSessions { get; set; } = new HashSet<Session>();
    }
}
