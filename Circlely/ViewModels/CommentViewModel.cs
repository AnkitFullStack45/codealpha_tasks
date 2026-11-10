using System.ComponentModel.DataAnnotations;

namespace Circlely.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}