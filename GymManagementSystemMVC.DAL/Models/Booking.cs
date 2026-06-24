using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace GymManagementSystemMVC.DAL.Models
    {
    public class Booking : BaseEntity
        {
        

        public DateTime BookingDate { get; set; }
        
        public bool IsAttended { get; set; } = false;
       
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;
        }
    }
