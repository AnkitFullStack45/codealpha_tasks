using Microsoft.AspNetCore.Identity;

namespace Circlely.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? Bio { get; set; }
        public string? ProfileImage { get; set; }
        public DateTime createdAt { get; set; } = DateTime.UtcNow;
        public bool IsPrivate { get; set; } = false;

    }
}
