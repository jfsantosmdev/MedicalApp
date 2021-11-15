using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.WebApp.Models
{
    public class SearchAppointmentViewModel
    {
        [Display(Name = "Clínica")]
        public int? ClincId { get; set; }
        [Display(Name = "Doctor")]
        public int? DoctorId { get; set; }
        [Display(Name = "Paciente")]
        public int? PatientId { get; set; } 
        [Required(ErrorMessage = "Campo obligatorio")]
        public DateTime? DateStart { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public DateTime? DateEnd { get; set; }
        public List<SelectListItem> ListOfClinics { get; set; }
        public List<SelectListItem> ListOfDoctors { get; set; }
        public List<SelectListItem> ListOfPatients { get; set; }
        public List<AppointmentViewModel> Appointments { get; set; }
    }
}
