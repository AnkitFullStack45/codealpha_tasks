using System.ComponentModel.DataAnnotations;

namespace ECommerceStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
        public string? Processor { get; set; }

        public string? RAM { get; set; }

        public string? Storage { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}