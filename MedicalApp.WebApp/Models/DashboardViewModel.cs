using System.Collections.Generic;

namespace MedicalApp.WebApp.Models
{
    public class DashboardViewModel
    {
        public int AppointmentsCount { get; set; }
        public int PatientsCount { get; set; }
        public int DoctorsCount { get; set; }
        public int ClinicsCount { get; set; }
        public List<AppointmentViewModel> Appointments { get; set; }
}
}
