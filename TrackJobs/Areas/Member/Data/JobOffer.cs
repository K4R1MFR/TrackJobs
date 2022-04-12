using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrackJobs.Areas.Admin.Data;

namespace TrackJobs.Areas.Member.Data
{
    public class JobOffer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GuId { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime? AppliedOn { get; set; }
        public string CompanyName { get; set; } = null!;
        //public string? CompanyLogo { get; set; }
        public string OfferTitle { get; set; } = null!;
        public string? Description { get; set; }
        public int SourceId { get; set; }
        public Source Source { get; set; } = null!;
        public string? LinkToOffer { get; set; }
        public bool HasSentResume { get; set; }
        public bool HasSentCoverLetter { get; set; }
        public List<Communication> Communications { get; set; } = new List<Communication>();
        public DateTime? InterviewDate { get; set; }
        public bool HasInterviewed { get; set; }
        public List<Contact> Contacts { get; set; } = new List<Contact>();
        public string? Salary { get; set; }
        public string? Perks { get; set; }
        public string? Pros { get; set; }
        public string? Cons { get; set; }
        public string? StreetNumber { get; set; }
        public string? StreetName { get; set; }
        public string? City { get; set; }
        public int? Postcode { get; set; }
        public string? State { get; set; }
        public bool IsWFHAvailable { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsSoftDeleted { get; set; }
        public bool IsRejected { get; set; }
        public string? RejectionFeedback { get; set; }
        public bool IsClosed { get; set; }
    }
}
