using ECommerceStore.Models;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
namespace ECommerceStore.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;
        public EmailService(IOptions<EmailSettings>options)
        {
            emailSettings = options.Value;
        }
        public bool SendEmail(string email, string subject, string message)
        {
            try
            {
                var mailmessage = new MimeMessage();
                mailmessage.From.Add(new MailboxAddress(emailSettings.Name,emailSettings.Email));
                mailmessage.To.Add(MailboxAddress.Parse(email));
                mailmessage.Subject = subject;
                var bodybuilder = new BodyBuilder()
                {
                    HtmlBody = message
                };
                mailmessage.Body = bodybuilder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(emailSettings.Host,emailSettings.Port);
                //Authenticate
                smtp.Authenticate(emailSettings.Email,emailSettings.Password);
                //send email
                smtp.Send(mailmessage);
                //disconnect
                smtp.Disconnect(true);
                return true;//because our send method is return type bool        
            }
            catch(Exception ex)
            {
                Console.WriteLine("EMAIL ERROR");
                Console.WriteLine(ex.ToString());
                return false;
            }          
        }
    }
}
