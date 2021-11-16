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
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DateStart { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DateEnd { get; set; }
        [Display(Name = "Estado")]
        public AppointmentStatus? Status { get; set; }
        public List<SelectListItem> ListOfClinics { get; set; }
        public List<SelectListItem> ListOfDoctors { get; set; }
        public List<SelectListItem> ListOfPatients { get; set; }
        public List<AppointmentViewModel> Appointments { get; set; }
    }
}
