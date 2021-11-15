﻿using MedicalApp.Entities;
using System;

namespace MedicalApp.WebApi.DTOs
{
    public class RegisterDoctorRequestDTO
    {
        
        public string Email { get; set; }
        public string Password { get; set; }

        #region Doctor
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public string Speciality { get; set; }
        public string Summary { get; set; }
        #endregion
    }
}
