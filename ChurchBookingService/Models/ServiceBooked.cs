using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchBookingService.Models
{
    public class ServiceBooked
    {
        public int Id { get; set; }
        public int ChurchDayId { get; set; }
        public int MemberId { get; set; }
        public int ServiceNo { get; set; }
        public int SeatNo { get; set; }

        public DateTime DateCreated { get; set; }
        public ChurchDay ChurchDay { get; set; }
        public Member Member { get; set; }
    }
}
