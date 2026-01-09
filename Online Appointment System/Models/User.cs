using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Online_Appointment_System.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
    }

}
