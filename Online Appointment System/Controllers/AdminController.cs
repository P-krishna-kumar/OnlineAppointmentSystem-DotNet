using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_Appointment_System.Data;
using Online_Appointment_System.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Appointment_System.Controllers
{
    public class AdminController : Controller
    {


        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ------------------------------
        // 1️⃣ Dashboard
        // ------------------------------
        public IActionResult Dashboard()
        {
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalAppointments = _context.Appointments.Count();
            ViewBag.PendingAppointments = _context.Appointments.Where(a => a.Status == "Pending").Count();
            ViewBag.TodaysAppointments = _context.Appointments
                .Where(a => a.AppointmentDate.Date == System.DateTime.Today).Count();

            return View();
        }

        // ------------------------------
        // 2️⃣ Manage Services
        // ------------------------------
        public IActionResult ManageServices()
        {
            var services = _context.Services.ToList();
            return View(services);
        }

        [HttpGet]
        public IActionResult AddService() => View();

        [HttpPost]
        public async Task<IActionResult> AddService(Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Add(service);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Service added successfully!";
                return RedirectToAction("ManageServices");
            }
            return View(service);
        }

        [HttpGet]
        public IActionResult EditService(int id)
        {
            var service = _context.Services.Find(id);
            if (service == null) return NotFound();
            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> EditService(Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Update(service);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Service updated successfully!";
                return RedirectToAction("ManageServices");
            }
            return View(service);
        }

        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Service deleted successfully!";
            }
            return RedirectToAction("ManageServices");
        }

        // ------------------------------
        // 3️⃣ Manage Time Slots
        // ------------------------------
        public IActionResult ManageTimeSlots()
        {
            var slots = _context.TimeSlots.Include(t => t.Service).ToList();
            ViewBag.Services = new SelectList(_context.Services.ToList(), "ServiceID", "ServiceName");
            return View(slots);
        }

        [HttpGet]
        public IActionResult AddTimeSlot()
        {
            ViewBag.Services = new SelectList(_context.Services.ToList(), "ServiceID", "ServiceName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTimeSlot(TimeSlot slot)
        {
            if (ModelState.IsValid)
            {
                _context.TimeSlots.Add(slot);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Time slot added successfully!";
                return RedirectToAction("ManageTimeSlots");
            }
            ViewBag.Services = new SelectList(_context.Services.ToList(), "ServiceID", "ServiceName", slot.ServiceID);
            return View(slot);
        }

        public async Task<IActionResult> ToggleSlotAvailability(int id)
        {
            var slot = await _context.TimeSlots.FindAsync(id);
            if (slot != null)
            {
                slot.IsAvailable = !slot.IsAvailable;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ManageTimeSlots");
        }

        // ------------------------------
        // 4️⃣ Manage Appointments
        // ------------------------------
        public IActionResult ManageAppointments(string status, int? serviceId)
        {
            var appointments = _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Service)
                .Include(a => a.TimeSlot)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                appointments = appointments.Where(a => a.Status == status);

            if (serviceId.HasValue)
                appointments = appointments.Where(a => a.ServiceID == serviceId.Value);

            ViewBag.Services = new SelectList(_context.Services.ToList(), "ServiceID", "ServiceName");
            return View(appointments.ToList());
        }

        public async Task<IActionResult> ApproveAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = "Approved";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ManageAppointments");
        }

        public async Task<IActionResult> CancelAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = "Cancelled";
                var slot = await _context.TimeSlots.FindAsync(appointment.SlotID);
                if (slot != null) slot.IsAvailable = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ManageAppointments");
        }

        // ------------------------------
        // 5️⃣ Manage Users
        // ------------------------------
        public IActionResult ManageUsers()
        {
            var users = _context.Users.Include(u => u.Roles).ToList();
            return View(users);
        }

        // Block / Unblock user
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                if (user.RoleID != 0)
                    user.RoleID = 0; // Block
                else
                    user.RoleID = 2; // Unblock (assuming 2 = User)

                await _context.SaveChangesAsync();
                TempData["Success"] = user.RoleID == 0 ? "User blocked successfully!" : "User unblocked successfully!";
            }
            return RedirectToAction("ManageUsers");
        }

    }
}
