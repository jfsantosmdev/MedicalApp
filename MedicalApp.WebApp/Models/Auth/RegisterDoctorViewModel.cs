using MedicalApp.WebApp.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalApp.WebApp.Models.Auth
{
    public class RegisterDoctorViewModel
    {
        [Required(ErrorMessage = "Ingrese un nombre")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Ingrese un apellido")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }
        [MinimumAge(18, ErrorMessage = "{0} debe ser alguien de al menos {1} años de edad")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Ingrese una fecha de nacimiento")]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime? BirthDate { get; set; }
        [Required(ErrorMessage = "Seleccione un género")]
        [Display(Name = "Género")]
        public Gender Gender { get; set; }
        //[Required(ErrorMessage = "Ingrese un número de teléfono")]
        [Display(Name = "Teléfono")]
        public string Phone { get; set; }
        //[Required(ErrorMessage = "Ingrese una especialidad")]
        [Display(Name = "Especialidad")]
        public string Speciality { get; set; }
        [Display(Name = "Resumen")]
        public string Summary { get; set; }
        [Display(Name = "¿Está disponible?")]
        public bool IsAvilable { get; set; }
        public RegisterViewModel Register { get; set; }
    }
}
