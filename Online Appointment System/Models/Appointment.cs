using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Appointment_System.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public int TimeSlotId { get; set; }
        public string AppointmentDate { get; set; }
    }
}
