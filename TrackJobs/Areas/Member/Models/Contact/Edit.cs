namespace TrackJobs.Areas.Member.Models.Contact
{
    public class Edit
    {
        public int Id { get; set; }
        public Guid JobOfferId { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string? Title { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}