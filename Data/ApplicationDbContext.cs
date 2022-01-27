using EdPlatformWebsite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EdPlatformWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Module>? Modules { get; set; }
        public DbSet<Lesson>? Lessons { get; set; }
        public DbSet<Exercise>? Exercises { get; set; }
        public DbSet<IOCase>? IOCases { get; set; }
        public DbSet<LastExerciseAttempt>? LastExerciseAttempts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}