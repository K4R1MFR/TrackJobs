namespace TrackJobs.Areas.Member.Models.JobOffer
{
    public class Edit
    {
        public Guid GuId { get; set; }
        public string? UserId { get; set; }
        public DateTime? AppliedOn { get; set; }
        public string CompanyName { get; set; } = null!;
        public string OfferTitle { get; set; } = null!;
        public int SourceId { get; set; }
        public string? LinkToOffer { get; set; }
        public bool HasSentResume { get; set; }
        public bool HasSentCoverLetter { get; set; }
        public string? Salary { get; set; }
        public string? Perks { get; set; }
        public string? Pros { get; set; }
        public string? Cons { get; set; }
        public string? StreetNumber { get; set; }
        public string? StreetName { get; set; }
        public string? City { get; set; }
        public int? Postcode { get; set; }
        public string? State { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime? InterviewDate { get; set; }
        public bool HasInterviewed { get; set; }


    }
}
