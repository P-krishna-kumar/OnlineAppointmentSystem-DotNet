using Microsoft.AspNetCore.Mvc;
using Online_Appointment_System.Models;
using System.Data;
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
         
public IActionResult Edit(int id)
    {
        DataTable dt = (DataTable)_slotDal.GetById(id);

        if (dt.Rows.Count == 0)
            return NotFound();

        TimeSlot model = new TimeSlot
        {
            TimeSlotId = id,
            SlotFrom = dt.Rows[0]["SlotFrom"].ToString(),
            SlotTo = dt.Rows[0]["SlotTo"].ToString()
        };

        return View(model);
    }


    [HttpPost]
        public IActionResult Edit(TimeSlot model)
        {
            if (ModelState.IsValid)
            {
                _slotDal.UpdateTimeSlot(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Delete(int id)
        {
            _slotDal.DeleteTimeSlot(id);
            return RedirectToAction("Index");
        }

        public IActionResult ToggleStatus(int id)
        {
            _slotDal.ToggleStatus(id);
            return RedirectToAction("Index");
        }



    }
}
