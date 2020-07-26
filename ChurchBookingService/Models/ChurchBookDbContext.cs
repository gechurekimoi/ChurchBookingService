using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchBookingService.Models
{
    public class ChurchBookDbContext:DbContext
    {
        public ChurchBookDbContext(DbContextOptions<ChurchBookDbContext> options) : base(options)
        {

        }

        public DbSet<Member> Members { get; set; }
        public DbSet<ChurchDay> ChurchDay { get; set; }
        public DbSet<ServiceBooked> ServiceBooked { get; set; }
        public DbSet<PermanentMember> PermanentMember { get; set; }
    }
}
