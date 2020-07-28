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

                    if(db.ServiceBooked.Include(p=>p.Member).Where(p=>p.ChurchDayId == booking.ChurchDayId && p.ServiceNo == booking.ServiceNo && p.Member.FullName == booking.FullName && p.Member.Age == booking.Age && p.Member.PhoneNumber == booking.PhoneNumber && p.Member.Residence == booking.Residence).Any())
                    {
                        return Json("MemberAlreadyBooked");
                    }

                    Member member = new Member()
                    {
                        FullName = booking.FullName,
                        Age = booking.Age,
                        DateCreated = DateTime.Now,
                        Gender = booking.Gender,
                        PhoneNumber = booking.PhoneNumber,
                        Residence = booking.Residence
                    };


                    member.DateCreated = DateTime.Now;
                    db.Members.Add(member);
                    db.SaveChanges();

                    ServiceBooked serviceBooked = new ServiceBooked()
                    {
                        ChurchDayId = booking.ChurchDayId,
                        MemberId = member.Id,
                        ServiceNo = booking.ServiceNo,
                        DateCreated = DateTime.Now,
                        SeatNo = GetSeatNo(booking.ChurchDayId,booking.ServiceNo)
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
                churchDay.RegistrationDeadline = churchDay.RegistrationDeadline.Value.AddHours(churchDay.RegistrationDeadlineTime.Value.Hour);
                churchDay.RegistrationDeadline = churchDay.RegistrationDeadline.Value.AddMinutes(churchDay.RegistrationDeadlineTime.Value.Minute);

                db.ChurchDay.Add(churchDay);
                db.SaveChanges();

                //here we get all permanent members and add them to this added service 
                var permementMembers = db.PermanentMember.ToList();

                if (churchDay.NoOfServices > 0)
                {
                    foreach (var member in permementMembers)
                    {
                        for (int i = 1; i <= churchDay.NoOfServices; i++)
                        {

                            var actualMember = new Member()
                            {
                                Age = member.Age,
                                DateCreated = DateTime.Now,
                                FullName = member.FullName,
                                Gender = member.Gender,
                                PhoneNumber = member.PhoneNumber,
                                Residence = member.Residence

                            };

                            db.Members.Add(actualMember);
                            db.SaveChanges();


                            ServiceBooked serviceBooked = new ServiceBooked()
                            {
                                ChurchDayId = churchDay.Id,
                                MemberId = actualMember.Id,
                                ServiceNo = i,
                                DateCreated = DateTime.Now,
                                SeatNo = member.SeatNo
                            };

                            db.ServiceBooked.Add(serviceBooked);
                        }
                    }
                }

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

        public IActionResult AddPermanentMember(PermanentMember member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    member.FullName = member.Title + " " + member.FullName;
                    member.DateCreated = DateTime.Now;
                    db.PermanentMember.Add(member);
                    db.SaveChanges();

                    var actualMember = new Member()
                    {
                        Age = member.Age,
                        DateCreated = DateTime.Now,
                        FullName = member.FullName,
                        Gender = member.Gender,
                        PhoneNumber = member.PhoneNumber,
                        Residence = member.Residence

                    };

                    db.Members.Add(actualMember);
                    db.SaveChanges();

                    var churchDay = db.ChurchDay.OrderByDescending(p => p.Id).FirstOrDefault();

                    if (churchDay.NoOfServices > 0)
                    {
                        for (int i = 1; i <= churchDay.NoOfServices; i++)
                        {
                            int seatNo = 0;

                            if (member.SeatNo > 0)
                            {
                                seatNo = member.SeatNo;
                            }


                            ServiceBooked serviceBooked = new ServiceBooked()
                            {
                                ChurchDayId = churchDay.Id,
                                MemberId = actualMember.Id,
                                ServiceNo =i,
                                DateCreated = DateTime.Now,
                                SeatNo = seatNo
                            };

                            db.ServiceBooked.Add(serviceBooked);
                        }
                    }

                    db.SaveChanges();

                    return Json("success");
                }
                return Json("InvalidModel");
            }
            catch (Exception ex)
            {

                return Json("error");
            }
            
        }

        public IActionResult GetPermanentMembers()
        {
            var data = db.PermanentMember.ToList();

            return View(data);
        }


        public int GetSeatNo(int churchDayId, int ServiceNo)
        {
            int seatNo = 0;

            var service = db.ServiceBooked
                            .Where(p => p.ChurchDayId == churchDayId && p.ServiceNo == ServiceNo)
                            .OrderByDescending(x=>x.Id).FirstOrDefault();

            if(service!= null)
            {
                if(service.SeatNo!=0 && service.SeatNo > 14)
                {
                    seatNo = service.SeatNo + 1;
                }
                else
                {
                    seatNo = 15;
                }
            }
            else
            {
                seatNo = 15;
            }



            return seatNo;
        }

        [HttpPost]
        public ActionResult DeleteChurchDay(int churchDayId)
        {
            var churchDay = db.ChurchDay.Where(p => p.Id == churchDayId).FirstOrDefault();

            var bookedServices = db.ServiceBooked.Where(p => p.ChurchDayId == churchDayId).FirstOrDefault();

            db.ServiceBooked.RemoveRange(bookedServices);
            db.ChurchDay.Remove(churchDay);

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
