using System;

namespace MedicalApp.WebApi.DTOs
{
    public class PatientDTO
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public string ProfileImage { get; set; }
    }
}
