namespace TrackJobs.Areas.Member.Models.Communication
{
    public class Edit
    {
        public int Id { get; set; }
        public Guid JobOfferId { get; set; }
        public DateTime Date { get; set; }
        public string CommunicationType { get; set; } = null!; //sent or received
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
