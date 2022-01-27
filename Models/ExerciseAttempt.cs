using Microsoft.AspNetCore.Identity;

namespace EdPlatformWebsite.Models
{
    public class LastExerciseAttempt
    {
        public int Id { get; set; }
        public IdentityUser? User { get; set; }
        public Exercise? Exercise { get; set; }
        public bool Successful { get; set; } = false;
        public string? Code { get; set; }
    }
}
