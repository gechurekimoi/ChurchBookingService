using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchBookingService.Models
{
    public class PermanentMember:Member
    {
        public string Title { get; set; }
        public int SeatNo { get; set; }
    }
}
