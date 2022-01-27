namespace EdPlatformWebsite.Models
{
    public class Module
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string? Name { get; set; }
        public ICollection<Lesson>? Lessons { get; set; }
    }
}
