namespace EdPlatformWebsite.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public int ModuleId { get; set; }
        public ICollection<Exercise>? Exercises { get; set; }
    }
}
