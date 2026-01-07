using System;
using System.Collections.Generic;

namespace Online_Appointment_System.Models
{
    public class AppointmentViewModel
    {
        public int AppointmentID { get; set; }
        public string ServiceName { get; set; }
        public DateTime SlotDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; }
    }

    public class BookAppointmentModel
    {
        public int ServiceID { get; set; }
        public DateTime SelectedDate { get; set; }
        public int SlotID { get; set; }
        public List<TimeSlot> AvailableSlots { get; set; }
    }
}
