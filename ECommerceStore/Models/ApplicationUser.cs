using Microsoft.AspNetCore.Identity;

namespace ECommerceStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

        public string? Address { get; set; }

        public string? ProfileImageUrl { get; set; }
    }
}
