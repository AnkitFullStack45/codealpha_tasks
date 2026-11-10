using System.ComponentModel.DataAnnotations;

namespace ECommerceStore.ViewModels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Please enter your full name.")]
        public string? FullName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? UserName { get; set; }

        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? ProfileImageUrl { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}