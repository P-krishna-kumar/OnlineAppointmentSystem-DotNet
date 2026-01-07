using System.ComponentModel.DataAnnotations;

namespace Online_Appointment_System.Models
{
    public class AccountViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
