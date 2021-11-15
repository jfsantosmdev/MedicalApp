using MedicalApp.WebApp.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.WebApp.Models
{
    public class DoctorViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }
        [MinimumAge(18, ErrorMessage = "{0} debe ser alguien de al menos {1} años de edad")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime? BirthDate { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Género")]
        public Gender Gender { get; set; }
        [Display(Name = "Teléfono")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Especialidad")]
        public string Speciality { get; set; }
        [Display(Name = "Resumen")]
        public string Summary { get; set; }
        [Display(Name = "¿Está Activo?")]
        public bool IsAvailable { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
