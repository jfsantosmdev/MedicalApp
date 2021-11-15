using MedicalApp.Entities;
using System;

namespace MedicalApp.WebApi.DTOs
{
    public class AppointmentDTO
    {
        public int Id { get; internal set; }
        public int ClinicId { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime DateTimeStart { get; set; }
        public DateTime DateTimeEnd { get; set; }
        public string Reason { get; set; }
        public string Note { get; set; }
        public int? UserId { get; set; }
    }
}
