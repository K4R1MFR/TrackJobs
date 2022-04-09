using System.ComponentModel.DataAnnotations.Schema;

namespace TrackJobs.Areas.Member.Data
{
    public class Communication
    {
        public int Id { get; set; }
        public Guid JobOfferId { get; set; }
        public JobOffer JobOffer { get; set; } = null!;
        public DateTime Date { get; set; }
        public string CommunicationType { get; set; } = null!; //sent or received
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}