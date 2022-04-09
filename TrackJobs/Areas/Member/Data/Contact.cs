using System.ComponentModel.DataAnnotations.Schema;

namespace TrackJobs.Areas.Member.Data
{
    public class Contact
    {
        public int Id { get; set; }
        public Guid JobOfferId { get; set; }
        public JobOffer JobOffer { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string? Title { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}