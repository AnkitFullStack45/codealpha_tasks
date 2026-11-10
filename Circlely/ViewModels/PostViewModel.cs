using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Circlely.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please write something.")]
        public string Content { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }

        public string? ImageUrl { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string? ProfileImage { get; set; }

        public DateTime CreatedAt { get; set; }

        public int LikeCount { get; set; }

        public int CommentCount { get; set; }

        public bool IsLikedByCurrentUser { get; set; }

        // All comments belonging to this post
        public List<CommentViewModel> Comments { get; set; }
            = new List<CommentViewModel>();
    }
}
