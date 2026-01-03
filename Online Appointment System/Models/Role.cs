namespace Online_Appointment_System.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        //// Navigation
        //public ICollection<User> Users { get; set; }
    }
}
