namespace MedicalApp.Entities
{
    public class AppointmentFile : Entity
    {
        public int AppointmentId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
