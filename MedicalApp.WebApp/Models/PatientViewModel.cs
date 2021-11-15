using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.WebApp.Models
{
    public class PatientViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Género")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Teléfono")]
        public string Telephone { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Dirección")]
        public string Address { get; set; }
        public string ProfileImage { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
