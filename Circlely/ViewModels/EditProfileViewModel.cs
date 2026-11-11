using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Circlely.ViewModels
{
    public class EditProfileViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(150)]
        public string? Bio { get; set; }

        public string? ProfileImage { get; set; }

        public IFormFile? Image { get; set; }
    }
}
