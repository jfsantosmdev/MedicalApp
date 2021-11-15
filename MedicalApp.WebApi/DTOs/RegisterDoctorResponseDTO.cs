using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalApp.WebApi.DTOs
{
    public class RegisterDoctorResponseDTO
    {
        public bool Register { get; set; }
        public List<string> Errors { get; set; }
    }
}
