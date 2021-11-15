using System;

namespace MedicalApp.Entities
{
    public class Appointment : Entity
    {
        public int ClinicId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public int? UserId { get; set; }
        public Clinic Clinic { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public AppointmentStatus Status { get; set; }
        public decimal? Cost { get; set; }

    }
}
