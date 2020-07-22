using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchBookingService.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Age { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
