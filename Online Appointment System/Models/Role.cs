using System.ComponentModel.DataAnnotations;

namespace Online_Appointment_System.Models
{
    public class Role
    {
    
        public int RoleID { get; set; }
        [Required, MaxLength(50)]
        public string RoleName { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
