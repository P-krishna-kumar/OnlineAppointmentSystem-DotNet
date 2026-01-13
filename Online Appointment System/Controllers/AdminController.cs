using Microsoft.AspNetCore.Mvc;
using Online_Appointment_System.DAL;

namespace Online_Appointment_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppointmentDAL _apptDal;
        private readonly AppointmentDAL _email;
        private readonly UserDAL _userDal;

        public AdminController(UserDAL userDal)
        {
            _userDal = userDal;
        }
        public AdminController(AppointmentDAL apptDal)
        {
            _apptDal = apptDal;
        }
        private readonly IConfiguration _config;

        public AdminController(IConfiguration config)
        {
            _config = config;
        }
        // Appointment List
        public IActionResult Appointments()
        {
            var dt = _apptDal.AdminAppointments();
            return View(dt);
        }

        // Update Status
        public IActionResult UpdateStatus(int id, string status)
        {
            _apptDal.UpdateStatus(id, status);

            string email = _apptDal.GetUserEmailByAppointment(id);
            string body = $"<h3>Your Appointment Status Updated</h3><p>Status: <strong>{status}</strong></p>";

            _email.SendEmail(email, "Appointment Status Update", body);

            return RedirectToAction("Appointments");
        }
        public IActionResult Index()
        {
            AdminDAL dal = new AdminDAL(_config);
            var dt = dal.DashboardStats();

            ViewBag.TotalUsers = dt.Rows[0]["TotalUsers"];
            ViewBag.TotalServices = dt.Rows[0]["TotalServices"];
            ViewBag.TotalAppointments = dt.Rows[0]["TotalAppointments"];
            ViewBag.TodayAppointments = dt.Rows[0]["TodayAppointments"];
            ViewBag.PendingCount = dt.Rows[0]["PendingCount"];
            ViewBag.ApprovedCount = dt.Rows[0]["ApprovedCount"];
            ViewBag.CompletedCount = dt.Rows[0]["CompletedCount"];

            return View();
        }


        // User List Page
        public IActionResult Users()
        {
            var dt = _userDal.GetAllUsers();
            return View(dt);
        }

        // Block User
        public IActionResult Block(int id)
        {
            _userDal.BlockUser(id);
            return RedirectToAction("Users");
        }

        // Unblock User
        public IActionResult Unblock(int id)
        {
            _userDal.UnblockUser(id);
            return RedirectToAction("Users");
        }

        // Delete User
        public IActionResult Delete(int id)
        {
            _userDal.DeleteUser(id);
            return RedirectToAction("Users");
        }


    }
}
