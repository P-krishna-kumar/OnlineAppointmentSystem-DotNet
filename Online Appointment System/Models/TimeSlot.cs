using System.ComponentModel.DataAnnotations;

namespace Online_Appointment_System.Models
{
    public class TimeSlot
    {
        public int SlotId { get; set; }

        public int ServiceId { get; set; }

        [DataType(DataType.Date)]
        public DateTime SlotDate { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        public bool IsAvailable { get; set; } = true;

        // Navigation
        public Service Service { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
