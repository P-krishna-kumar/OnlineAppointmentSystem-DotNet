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

    }
}
