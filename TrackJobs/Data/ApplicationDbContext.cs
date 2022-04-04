using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackJobs.Areas.Member.Data;

namespace TrackJobs.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<JobOffer>? JobOffers { get; set; }
        public DbSet<Source>? Sources { get; set; }
        public DbSet<Contact>? Contacts { get; set; }
        public DbSet<Communication>? Communications { get; set; }
    }
}