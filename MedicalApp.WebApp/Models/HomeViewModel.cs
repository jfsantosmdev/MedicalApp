using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalApp.WebApp.Models
{
    public class HomeViewModel
    {
        public int ScheduledAppointmentsCount { get; set; }
        public int ConfirmedAppointmentsCount { get; set; }
        public int CancelledAppointmentsCount { get; set; }
        public int FinishedAppointmentsCount { get; set; }
        public List<AppointmentViewModel> Appointments { get; set; }
    }
}
