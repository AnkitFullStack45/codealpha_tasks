using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerceStore.ViewModels
{
    public class ResetPasswordViewModel
    {
        [DisplayName("Email Address")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [DisplayName("New Password")]
        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "New password and confirm password should match.")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }

    }
}
