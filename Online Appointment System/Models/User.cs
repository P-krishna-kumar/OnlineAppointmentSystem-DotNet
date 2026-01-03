using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Online_Appointment_System.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public required string Name { get; set; }

        [Required, MaxLength(100)]
        public required string Email { get; set; }

        [Required, MaxLength(100)]
        public required string Password { get; set; }

        public int RoleId { get; set; }

        // Navigation
        public required string Role { get; set; }
        public required ICollection<Appointment> Appointments { get; set; }
    }
}
