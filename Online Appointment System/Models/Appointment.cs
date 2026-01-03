using System;
using System.ComponentModel.DataAnnotations;

namespace Online_Appointment_System.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public int UserID { get; set; }
        public int ServiceID { get; set; }
        public int SlotID { get; set; }

        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        //// Navigation
        //public User User { get; set; }
        //public Service Service { get; set; }
        //public TimeSlot TimeSlot { get; set; }
    }
}
