using Microsoft.EntityFrameworkCore;
using Online_Appointment_System.Data;
using Online_Appointment_System.Models;

namespace Online_Appointment_System.Services
{
    public class AppointmentService
    {
        private readonly ApplicationDbContext _context;

        public AppointmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Fetch available slots using SP
        public async Task<List<TimeSlot>> GetAvailableSlots(int serviceId, DateTime date)
        {
            return await _context.TimeSlots
                .FromSqlRaw("EXEC GetAvailableTimeSlots @ServiceId={0}, @Date={1}", serviceId, date)
                .ToListAsync();
        }
    }
}
