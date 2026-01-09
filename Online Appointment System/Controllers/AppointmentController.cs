using Microsoft.AspNetCore.Mvc;
using Online_Appointment_System.DAL;
using Online_Appointment_System.Models;
using static Online_Appointment_System.DAL.TimeSlatDAL;

namespace Online_Appointment_System.Controllers
{
    public class AppointmentController : Controller
    {

        private readonly AppointmentDAL _appointmentDAL;
        private readonly ServiceDAL _serviceDAL;
        private readonly TimeSlotDAL _slotDAL;

        public AppointmentController(AppointmentDAL appointmentDAL, ServiceDAL serviceDAL, TimeSlotDAL slotDAL)
        {
            _appointmentDAL = appointmentDAL;
            _serviceDAL = serviceDAL;
            _slotDAL = slotDAL;
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Services = _serviceDAL.ListService();
            ViewBag.TimeSlots = _slotDAL.ListTimeSlot();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Appointment model)
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToAction("Login", "Account");

            model.UserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

            _appointmentDAL.AddAppointment(model);
            return RedirectToAction("MyAppointments");
        }

        public IActionResult MyAppointments()
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            var dt = _appointmentDAL.GetUserAppointments(userId);
            return View(dt);
        }
    }
}
