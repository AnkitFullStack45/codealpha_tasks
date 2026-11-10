using System.ComponentModel.DataAnnotations;

namespace ECommerceStore.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
