namespace ECommerceStore.Models
{
    public class RecaptchaResponse
    {
        public bool Success { get; set; }
        public string[]? ErrorCodes { get; set; }
    }
}
