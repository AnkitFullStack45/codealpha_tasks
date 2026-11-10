using Circlely.Models;

namespace Circlely.ViewModels
{
    public class ProfileViewModel
    {
        public string Id { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Bio { get; set; }

        public string? ProfileImage { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsPrivate { get; set; }

        public int PostCount { get; set; }

        public int FollowerCount { get; set; }

        public int FollowingCount { get; set; }
        public List<Post> Posts { get; set; } = new();
    }
}