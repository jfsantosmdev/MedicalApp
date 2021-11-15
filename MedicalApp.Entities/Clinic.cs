using System.Collections.Generic;

namespace MedicalApp.Entities
{
    public class Clinic : Entity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string WebSite { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public bool IsActive { get; set; }
    }
}
