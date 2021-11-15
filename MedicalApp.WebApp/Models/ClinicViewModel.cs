using System.ComponentModel.DataAnnotations;

namespace MedicalApp.WebApp.Models
{
    public class ClinicViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo es obligatorio")]
        [Display(Name = "Dirección")]
        public string Address { get; set; }
        [Display(Name = "Sitio Web")]
        [Url(ErrorMessage = "El campo es obligatorio")]
        public string WebSite { get; set; }
        [Display(Name = "Correo Electrónico")]
        [EmailAddress(ErrorMessage = "Ingrese una dirección de correo valida")]
        public string Email { get; set; }
        [Display(Name = "Teléfono")]
        public string Telephone { get; set; }
        [Display(Name = "¿Está Activo?")]
        public bool IsActive { get; set; }
    }
}
