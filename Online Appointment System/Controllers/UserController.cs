using Microsoft.AspNetCore.Mvc;
using Online_Appointment_System.DAL;
using Online_Appointment_System.Models;

namespace Online_Appointment_System.Controllers
{
    public class UserController : Controller
    {
        private readonly UserDAL _userDal;
        private readonly UserDAL _dal;

         

        public UserController(UserDAL userDal, UserDAL dal)
        {
            _userDal = userDal;
            _dal = dal;
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToAction("Login", "Account");

            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            var dt = _userDal.UserDashboardStats(userId);

            ViewBag.TotalAppointments = dt.Rows[0]["TotalAppointments"];
            ViewBag.Upcoming = dt.Rows[0]["UpcomingAppointment"];
            ViewBag.Last = dt.Rows[0]["LastAppointment"];
            ViewBag.Pending = dt.Rows[0]["PendingCount"];
            ViewBag.Approved = dt.Rows[0]["ApprovedCount"];
            ViewBag.Completed = dt.Rows[0]["CompletedCount"];

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");

            return View();
        }
        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int uid = Convert.ToInt32(
                HttpContext.Session.GetString("UserId"));

            var user = _context.Users
                .FirstOrDefault(x => x.UserId == uid);

            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateProfile(User model)
        {
            var user = _context.Users.Find(model.UserId);

            user.FullName = model.FullName;

            _context.SaveChanges();

            TempData["msg"] = "Profile Updated Successfully";

            return RedirectToAction("Profile");
        }


    }
}
