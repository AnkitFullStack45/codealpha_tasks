using System.ComponentModel.DataAnnotations;

namespace Circlely.Models
{
    public class Story
    {
        public int Id { get; set; }

        // User who created the story
        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;

        // Image used in the story
        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        // Optional story caption
        [MaxLength(500)]
        public string? Caption { get; set; }

        // Story creation time
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Story automatically expires after 24 hours
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddHours(24);
    }
}