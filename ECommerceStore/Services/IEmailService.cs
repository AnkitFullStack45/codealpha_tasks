namespace ECommerceStore.Services
{
    public interface IEmailService
    {
        public bool SendEmail(string email, string subject, string message);
       
    }
}
