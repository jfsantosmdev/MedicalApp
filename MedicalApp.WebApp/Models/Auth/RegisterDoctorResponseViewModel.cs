using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalApp.WebApp.Models.Auth
{
    public class RegisterDoctorResponseViewModel
    {
        public bool Register { get; set; }
        public List<string> Errors { get; set; }
    }
}
