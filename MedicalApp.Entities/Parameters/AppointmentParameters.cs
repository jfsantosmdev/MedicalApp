using System;

namespace MedicalApp.Entities.Parameters
{
    public class AppointmentParameters : QueryStringParameters
    {
        public DateTime? DateTimeStart { get; set; }
        public DateTime? DateTimeEnd { get; set; }
        public bool ValidRange => DateTimeStart <= DateTimeEnd;
        public int? ClinicId { get; set; }
        public int? DoctorId { get; set; }
        public int? PatientId { get; set; }
    }
}
