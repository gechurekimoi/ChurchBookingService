using ChurchBookingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchBookingService.ViewModels
{
    public class BookingModel:Member
    {
        //public Member Member { get; set; }
        public int ChurchDayId { get; set; }
        public int ServiceNo { get; set; }
       
    }
}
