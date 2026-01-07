using System.ComponentModel.DataAnnotations;

namespace Online_Appointment_System.Models
{
    public class Service
    {
        [Key]
        public int ServiceID { get; set; }
        [Required, MaxLength(150)]
        public string ServiceName { get; set; }
        public string Description { get; set; }

        public ICollection<TimeSlot> TimeSlots { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
