using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_Appointment_System.Data;
using Online_Appointment_System.Models;
using System;
using System.Linq;

namespace Online_Appointment_System.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Dashboard – Overview of appointments
        public IActionResult Dashboard()
        {
            var userEmail = User.Identity.Name;
            var user = _context.Users
                        .Include(u => u.Appointments)
                        .ThenInclude(a => a.Service)
                        .FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var upcomingAppointments = user.Appointments
                                        .Where(predicate: a => a.SlotDate >= DateTime.Today)
                                        .ToList();

            ViewBag.TotalAppointments = user.Appointments.Count;
            ViewBag.UpcomingAppointments = upcomingAppointments.Count;

            return View(upcomingAppointments);
        }

        // 2️⃣ Book Appointment – Select Service + Date
        public IActionResult Book()
        {
            ViewBag.Services = _context.Services.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult GetAvailableSlots(int serviceId, DateTime selectedDate)
        {
            var slots = _context.TimeSlots
                        .Where(s => s.ServiceID == serviceId && s.SlotDate == selectedDate && s.IsAvailable)
                        .ToList();

            return PartialView("_AvailableSlotsPartial", slots);
        }

        [HttpPost]
        public IActionResult ConfirmBooking(int slotId)
        {
            var userEmail = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);

            var slot = _context.TimeSlots.Find(slotId);
            if (slot == null || !slot.IsAvailable)
                return BadRequest("Slot not available");

            var appointment = new Appointment
            {
                UserID = user.UserID,
                ServiceID = slot.ServiceID,
                SlotID = slot.SlotID,
                SlotDate = slot.SlotDate,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                Status = "Pending"
            };

            slot.IsAvailable = false;

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        // 3️⃣ Appointment History
        public IActionResult History()
        {
            var userEmail = User.Identity.Name;
            var user = _context.Users
                        .Include(u => u.Appointments)
                        .ThenInclude(a => a.Service)
                        .FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var appointments = user.Appointments
                                .OrderByDescending(a => a.SlotDate)
                                .ToList();

            return View(appointments);
        }

        // Cancel Appointment
        public IActionResult Cancel(int id)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment != null && appointment.Status == "Pending")
            {
                appointment.Status = "Cancelled";
                var slot = _context.TimeSlots.Find(appointment.SlotID);
                if (slot != null) slot.IsAvailable = true;

                _context.SaveChanges();
            }
            return RedirectToAction("History");
        }

        // 4️⃣ Profile – View & Edit
        public IActionResult Profile()
        {
            var userEmail = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            return View(user);
        }

        [HttpPost]
        public IActionResult Profile(User updatedUser)
        {
            var user = _context.Users.Find(updatedUser.UserID);
            if (user != null)
            {
                user.Name = updatedUser.Name;
                user.Email = updatedUser.Email;
                // Password update optional, can be handled separately
                _context.SaveChanges();
            }
            return RedirectToAction("Profile");
        }
    }
}
