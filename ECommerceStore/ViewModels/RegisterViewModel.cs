using System.ComponentModel.DataAnnotations;

namespace ECommerceStore.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter your full name.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter your email.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[6-9]\d{9}$",ErrorMessage = "Phone number must be exactly 10 digits and start with 6, 7, 8, or 9.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
