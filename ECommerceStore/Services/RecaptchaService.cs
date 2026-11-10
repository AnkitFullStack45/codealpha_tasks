using System.Text.Json;
using ECommerceStore.Models;

namespace ECommerceStore.Services
{
    public class RecaptchaService
    {
        private readonly IConfiguration configuration;
        private readonly HttpClient httpClient;

        public RecaptchaService(
            IConfiguration configuration,
            HttpClient httpClient)
        {
            this.configuration = configuration;
            this.httpClient = httpClient;
        }
        public async Task<bool> VerifyAsync(string captchaToken)
        {
            if (string.IsNullOrWhiteSpace(captchaToken))
            {
                return false;
            }

            var secretKey = configuration["Recaptcha:SecretKey"];

            var values = new Dictionary<string, string>
            {
                { "secret", secretKey! },
                { "response", captchaToken }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await httpClient.PostAsync(
                "https://www.google.com/recaptcha/api/siteverify",
                content);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<RecaptchaResponse>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result?.Success == true;
        }
    }
}