using MedicalApp.WebApp.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.WebApp.Models
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Clínica")]
        public int? ClinicId { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Doctor")]
        public int? DoctorId { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Paciente")]
        public int? PatientId { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Fecha/Hora Inicio")]
        [DateGreaterThan("DateTimeEnd")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime? DateTimeStart { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Fecha/Hora Fin")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [DataType(DataType.DateTime)]
        public DateTime? DateTimeEnd { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Razón")]
        public string Reason { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Nota")]

        public string Note { get; set; }
        public int? UserId { get; set; }
        [Display(Name = "Estado")]
        public AppointmentStatus Status { get; set; }
        public ClinicViewModel Clinic { get; set; }
        public DoctorViewModel Doctor { get; set; }
        public PatientViewModel Patient { get; set; }

        public List<SelectListItem> ListOfClincs { get; set; }
        public List<SelectListItem> ListOfDoctors { get; set; }
        public List<SelectListItem> ListOfPatients { get; set; }
    }

    public enum AppointmentStatus
    {
        Programada = 1,
        Confirmada = 2,
        Finalizada = 3,
        Cancelada = 4
    }
}
