using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Online_Appointment_System.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Required, MaxLength(150)]
        public string Name { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public int RoleID { get; set; }

        public Roles Roles { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }

    public enum Roles
    {
        Admin,
        User
    }


}
