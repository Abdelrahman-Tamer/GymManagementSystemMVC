using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemMVC.DAL.Models
    {
    public class Member : GymUser
        {

        public string? Photo { get; set; }   

        public DateTime JoinDate { get; set; }  

        public HealthRecord? HealthRecord { get; set; }

        public ICollection<MemberShip> MemberShips { get; set; } = new List<MemberShip>();

        public ICollection<Booking> Bookings { get; set; }= new List<Booking>();
        public ICollection<Booking> MemberSessions { get; set; } = new HashSet<Booking>();
        public ICollection<MemberShip> MemberPlans { get; set; } = new HashSet<MemberShip>();
        }
    }
