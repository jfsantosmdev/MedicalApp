using MedicalApp.Abstractions;

namespace MedicalApp.WebApi.Configuration
{
    public class MailParameters : IMailParameters
    {
        public string ToEmail { get ; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
