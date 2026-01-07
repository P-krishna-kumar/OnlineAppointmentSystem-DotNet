using System.ComponentModel.DataAnnotations;

namespace Online_Appointment_System.Models
{
    public class TimeSlot
    {
        [Key]
        public int SlotID { get; set; }
        public int ServiceID { get; set; }
        public DateTime SlotDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; }

        public Service Service { get; set; }
    }
}
