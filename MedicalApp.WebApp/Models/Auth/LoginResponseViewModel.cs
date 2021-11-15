using System.Collections.Generic;

namespace MedicalApp.WebApp.Models.Auth
{
    public class LoginResponseViewModel
    {
        public string Token { get; set; }
        public bool Login { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Errors { get; set; }
        public bool IsInRole(string roleName)
        {
            if(Roles != null)
                return Roles.Exists(r => r.ToUpper().Equals(roleName.ToUpper()));

            return false;
        }
    }
}
