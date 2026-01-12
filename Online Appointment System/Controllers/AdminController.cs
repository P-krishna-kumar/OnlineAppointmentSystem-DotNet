using Microsoft.AspNetCore.Mvc;
using Online_Appointment_System.DAL;

namespace Online_Appointment_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppointmentDAL _apptDal;

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




    }
}
