using System.ComponentModel.DataAnnotations;

namespace ECommerceStore.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Enter A Valid Email")]
        public string Email { get; set; } = string.Empty;
    }
}
