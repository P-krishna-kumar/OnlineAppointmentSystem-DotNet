using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Appointment_System.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }

        [Required]
        [Display(Name = "User")]
        public int UserID { get; set; } // FK to Users

        [Required]
        [Display(Name = "Service")]
        public int ServiceID { get; set; } // FK to Services

        [Required]
        [Display(Name = "Slot")]
        public int SlotID { get; set; } // FK to TimeSlots

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [ForeignKey("ServiceID")]
        public virtual Service Service { get; set; }

        [ForeignKey("SlotID")]
        public virtual TimeSlot Slot { get; set; }
        public DateTime SlotDate { get; internal set; }
        public TimeSpan StartTime { get; internal set; }
        public TimeSpan EndTime { get; internal set; }
        public object TimeSlot { get; internal set; }
    }
}
