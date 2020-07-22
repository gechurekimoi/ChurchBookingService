using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChurchBookingService.Models;
using ChurchBookingService.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ChurchBookingService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ChurchBookDbContext db;

        public HomeController(ILogger<HomeController> logger, ChurchBookDbContext _db)
        {
            _logger = logger;
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult BookService()
        {

            ViewData["ChurchDay"] = db.ChurchDay.OrderByDescending(p => p.Id).FirstOrDefault();

            return View();
        }

        [HttpPost]
        public IActionResult BookService(BookingModel booking)
        {
            try
            {

                var count = db.ServiceBooked.Where(p => p.ChurchDayId == booking.ChurchDayId && p.ServiceNo == booking.ServiceNo).Count();

                if(count < 100)
                {
                    //TODO: Add Validation to prevent Member from booking twice

                    Member member = new Member()
                    {
                        FullName = booking.FullName,
                        Age = booking.Age,
                        DateCreated = DateTime.Now,
                        Gender = booking.Gender,
                        PhoneNumber = booking.PhoneNumber
                    };


                    member.DateCreated = DateTime.Now;
                    db.Members.Add(member);
                    db.SaveChanges();

                    ServiceBooked serviceBooked = new ServiceBooked()
                    {
                        ChurchDayId = booking.ChurchDayId,
                        MemberId = member.Id,
                        ServiceNo = booking.ServiceNo,
                        DateCreated = DateTime.Now
                    };

                    db.ServiceBooked.Add(serviceBooked);
                    db.SaveChanges();

                    return Json("success");
                }
                else
                {
                    return Json("serviceFullyBooked");
                }

              

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json("error");
            }


        }

        public IActionResult SaveService(ChurchDay churchDay)
        {
            try
            {
                churchDay.DateCreated = DateTime.Now;

                db.ChurchDay.Add(churchDay);
                db.SaveChanges();

                return Json("success");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return Json("error");
            }
        }


        public IActionResult GetDashboard()
        {
            try
            {

                List<DashBoardData> dashBoardDatas = new List<DashBoardData>();
                using (var command = db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = $"EXEC Proc_GetDashBoardData";
                    db.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        // do something with result

                        while (result.Read())
                        {
                            DashBoardData dashBoardData = new DashBoardData();
                            dashBoardData.ChurchDayId = Convert.ToInt32(result["ChurchDayId"]);
                            dashBoardData.ServiceNo = Convert.ToInt32(result["ServiceNo"]);
                            dashBoardData.NumberOfPeople = Convert.ToInt32(result["NumberOfPeople"]);
                            dashBoardData.ServiceDate = Convert.ToDateTime(result["ServiceDate"]);

                            dashBoardDatas.Add(dashBoardData);

                        }

                    }
                }

                return PartialView("~/Views/Home/_DashboardStatistics.cshtml", dashBoardDatas);
            }
            catch (Exception ex)
            {
                List<DashBoardData> dashBoardDatas = new List<DashBoardData>();
                return PartialView("~/Views/Home/_DashboardStatistics.cshtml", dashBoardDatas);
            }
        }

        public IActionResult GetMembersPerService(int ChurchDayId, int ServiceNo)
        {
            try
            {
                var data = db.ServiceBooked
                        .Where(p => p.ChurchDayId == ChurchDayId && p.ServiceNo == ServiceNo)
                        .Include(x => x.Member)
                        .Include(y => y.ChurchDay)
                        .ToList();


                return View(data);
            }
            catch (Exception ex)
            {

                return View(new List<ServiceBooked>());
            }
           
        }
    }
}
