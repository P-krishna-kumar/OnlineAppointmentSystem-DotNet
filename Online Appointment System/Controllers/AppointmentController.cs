using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_Appointment_System.Data;
using Online_Appointment_System.Models;

namespace Online_Appointment_System.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Appointment
        public async Task<IActionResult> Index()
        {
            var appointments = _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Service)
                .Include(a => a.Slot);
            return View(await appointments.ToListAsync());
        }

        // GET: Appointment/Create
        public IActionResult Create()
        {
            ViewBag.Users = _context.Users.ToList();
            ViewBag.Services = _context.Services.ToList();
            ViewBag.Slots = _context.TimeSlots.ToList();
            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.CreatedAt = DateTime.Now;
                appointment.Status = "Pending";

                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Users = _context.Users.ToList();
            ViewBag.Services = _context.Services.ToList();
            ViewBag.Slots = _context.TimeSlots.ToList();
            return View(appointment);
        }

        // GET: Appointment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            ViewBag.Users = _context.Users.ToList();
            ViewBag.Services = _context.Services.ToList();
            ViewBag.Slots = _context.TimeSlots.ToList();
            return View(appointment);
        }

        // POST: Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment appointment)
        {
            if (id != appointment.AppointmentID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Appointments.Any(e => e.AppointmentID == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Users = _context.Users.ToList();
            ViewBag.Services = _context.Services.ToList();
            ViewBag.Slots = _context.TimeSlots.ToList();
            return View(appointment);
        }

        // GET: Appointment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Service)
                .Include(a => a.Slot)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // POST: Appointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public JsonResult GetAvailableSlots(DateTime date)
        {
            var bookedSlots = _context.Appointments
                .Where(a => a.AppointmentDate.Date == date.Date)
                .Select(a => a.SlotID)
                .ToList();

            var availableSlots = _context.TimeSlots
                .Where(s => !bookedSlots.Contains(s.SlotID))
                .Select(s => new { s.SlotID, s.StartTime, s.EndTime })
                .ToList();

            return Json(availableSlots);
        }

    }
}
