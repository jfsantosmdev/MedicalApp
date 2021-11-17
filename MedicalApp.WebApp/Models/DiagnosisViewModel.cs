using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalApp.WebApp.Models
{
    public class DiagnosisViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        public int AppointmentId { get; set; }
        [Display(Name = "Comentarios")]
        [MaxLength(1000, ErrorMessage = "No puede ingresar mas de 1000 carácteres")]
        [Required(ErrorMessage = "Ingrese un comentario")]
        public string Comments { get; set; }
        public AppointmentViewModel Appointment { get; set; }
        public List<DiagnosisFileViewModel> Files { get; set; }
        public DiagnosisViewModel()
        {
            Files = new List<DiagnosisFileViewModel>();
        }
    }
}
