namespace Circlely.Services
{
    public interface IEmailService
    {
        public bool SendMail(string email, string subject, string message);
    }
}
