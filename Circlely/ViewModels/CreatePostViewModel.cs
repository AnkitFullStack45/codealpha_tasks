using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Circlely.ViewModels
{
    public class CreatePostViewModel
    {
        [MaxLength(2200)]
        public string? Caption { get; set; }

        public IFormFile? Image { get; set; }
    }
}