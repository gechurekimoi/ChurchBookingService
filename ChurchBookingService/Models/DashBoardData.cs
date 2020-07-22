using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchBookingService.Models
{
    public class DashBoardData
    {
        public int ChurchDayId { get; set; }
        public int ServiceNo { get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime ServiceDate { get; set; }
    }
}
