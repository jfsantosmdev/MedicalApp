using MailKit.Net.Smtp;
using MailKit.Security;
using MedicalApp.Abstractions;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace MedicalApp.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(IMailParameters parameters);
    }
    public class MailService : IMailService
    {
        private readonly MailConfig _mailConfig;
        public MailService(IOptionsMonitor<MailConfig> optionsMonitor)
        {
            _mailConfig = optionsMonitor.CurrentValue;
        }

        public async Task SendEmailAsync(IMailParameters parameters)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_mailConfig.DisplayName, _mailConfig.Mail));
            email.To.Add(new MailboxAddress("", parameters.ToEmail));
            email.Subject = parameters.Subject;
            var builder = new BodyBuilder();
            //if (attachments != null)
            //{
            //    byte[] fileBytes;
            //    foreach (var file in mailRequest.Attachments)
            //    {
            //        if (file.Length > 0)
            //        {
            //            using (var ms = new MemoryStream())
            //            {
            //                file.CopyTo(ms);
            //                fileBytes = ms.ToArray();
            //            }
            //            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
            //        }
            //    }
            //}
            builder.HtmlBody = parameters.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailConfig.Host, _mailConfig.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailConfig.Mail, _mailConfig.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
