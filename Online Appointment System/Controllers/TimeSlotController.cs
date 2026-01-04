using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Appointment_System.Data;
using Online_Appointment_System.Models;

namespace Online_Appointment_System.Controllers
{
    public class TimeSlotController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TimeSlotController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TimeSlot
        public async Task<IActionResult> Index()
        {
            var slots = await _context.TimeSlots.Include(t => t.Service).ToListAsync();
            return View(slots);
        }

        // GET: TimeSlot/Create
        public IActionResult Create()
        {
            ViewBag.Services = _context.Services.ToList();
            return View();
        }

        // POST: TimeSlot/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimeSlot slot)
        {
            if (ModelState.IsValid)
            {
                _context.TimeSlots.Add(slot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Services = _context.Services.ToList();
            return View(slot);
        }

        // GET: TimeSlot/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var slot = await _context.TimeSlots.FindAsync(id);
            if (slot == null) return NotFound();

            ViewBag.Services = _context.Services.ToList();
            return View(slot);
        }

        // POST: TimeSlot/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TimeSlot slot)
        {
            if (id != slot.SlotID) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(slot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Services = _context.Services.ToList();
            return View(slot);
        }

        // GET: TimeSlot/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var slot = await _context.TimeSlots.Include(t => t.Service)
                .FirstOrDefaultAsync(t => t.SlotID == id);

            if (slot == null) return NotFound();

            return View(slot);
        }

        // POST: TimeSlot/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slot = await _context.TimeSlots.FindAsync(id);
            if (slot != null)
            {
                _context.TimeSlots.Remove(slot);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
