using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalApp.WebApi.DTOs
{
    public class DiagnosisDTO
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string Comments { get; set; }
    }
}
