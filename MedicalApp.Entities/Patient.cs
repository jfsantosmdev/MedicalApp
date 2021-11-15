using System;
using System.Collections.Generic;

namespace MedicalApp.Entities
{
    public class Patient : Entity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public string ProfileImage { get; set; }
    }
}
