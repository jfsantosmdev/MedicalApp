using System.Collections.Generic;

namespace MedicalApp.Entities
{
    public class Diagnosis : Entity
    {
        public int AppointmentId { get; set; }
        public string Comments { get; set; }
        public Appointment Appointment { get; set; }
        public IList<DiagnosisFile> Files { get; set; }
    }
}
