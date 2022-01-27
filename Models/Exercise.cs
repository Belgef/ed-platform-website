namespace EdPlatformWebsite.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<IOCase>? IOCases { get; set; }
    }
}
