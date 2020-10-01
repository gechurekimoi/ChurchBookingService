using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchBookingService.Models
{
    public class ChurchDay
    {
        public int Id { get; set; }
        public DateTime ServiceDate { get; set; }
        public int NoOfServices { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? RegistrationDeadline { get; set; }
        public DateTime? RegistrationDeadlineTime { get; set; }
        public string ServiceType { get; set; } 
    }
}
