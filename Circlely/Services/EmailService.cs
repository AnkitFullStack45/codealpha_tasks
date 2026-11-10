using Circlely.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;
namespace Circlely.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            emailSettings = options.Value;
        }

        public bool SendMail(string email, string subject, string message)
        {
            try
            {
                var mailMessage = new MimeMessage();

                mailMessage.From.Add(new MailboxAddress(
                    emailSettings.Name,
                    emailSettings.Email));

                mailMessage.To.Add(MailboxAddress.Parse(email));

                mailMessage.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = message
                };

                mailMessage.Body = bodyBuilder.ToMessageBody();

                using var smtp = new SmtpClient();

                smtp.Connect(
                    emailSettings.Host,
                    emailSettings.Port,
                    SecureSocketOptions.StartTls);

                smtp.Authenticate(
                    emailSettings.Email,
                    emailSettings.Password);

                smtp.Send(mailMessage);

                smtp.Disconnect(true);

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Email Error");
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}