using Microsoft.AspNetCore.Identity;

namespace EdPlatformWebsite.Models
{
    public class LastVisitedPage
    {
        public int Id { get; set; }
        public IdentityUser? User { get; set; }
        public string? Path { get; set; }
    }
}
