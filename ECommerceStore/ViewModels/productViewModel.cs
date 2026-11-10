using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ECommerceStore.ViewModels
{
    public class productViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string? Processor { get; set; }

        public string? RAM { get; set; }

        public string? Storage { get; set; }

        public IFormFile? ImageFile { get; set; }

        public string? Description { get; set; }
    }
}
