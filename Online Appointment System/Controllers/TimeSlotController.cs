using Microsoft.AspNetCore.Mvc;
using Online_Appointment_System.Models;
using static Online_Appointment_System.DAL.TimeSlatDAL;

namespace Online_Appointment_System.Controllers
{
    public class TimeSlotController : Controller
    {
        private readonly TimeSlotDAL _slotDal;

        public TimeSlotController(TimeSlotDAL slotDal)
        {
            _slotDal = slotDal;
        }

        public IActionResult Index()
        {
            var dt = _slotDal.ListTimeSlot();
            return View(dt);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TimeSlot model)
        {
            if (ModelState.IsValid)
            {
                _slotDal.AddTimeSlot(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }



         
    }
}
