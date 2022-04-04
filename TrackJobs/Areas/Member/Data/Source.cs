namespace TrackJobs.Areas.Member.Data
{
    public class Source
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!; //LinkedIn, Seek, Jora, etc
        public string? Logo { get; set; }
    }
}
