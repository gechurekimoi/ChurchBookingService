using ChurchBookingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchBookingService.ViewModels
{
    public class GetChurchDay
    {
        ChurchBookDbContext db;
        public GetChurchDay(ChurchBookDbContext _db)
        {
            db = _db;
        }
    }
}
