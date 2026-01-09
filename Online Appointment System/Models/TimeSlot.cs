namespace Online_Appointment_System.Models
{
    public class TimeSlot
    {
        public int TimeSlotId { get; set; }
        public string SlotFrom { get; set; }
        public string SlotTo { get; set; }
        public bool Status { get; set; }
    }
}
