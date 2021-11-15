using System.ComponentModel.DataAnnotations;

namespace MedicalApp.WebApp.Models.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Ingrese un correo electrónico")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Ingrese una contraseña")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recuérdame")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
