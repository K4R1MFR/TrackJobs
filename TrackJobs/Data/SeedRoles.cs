using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TrackJobs.Data
{
    public static class SeedRoles
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                string[] roles = new string[] { "Member", "Admin", "Demo" };

                var newrolelist = new List<IdentityRole>();
                foreach (string role in roles)
                {
                    if (!context.Roles.Any(r => r.Name == role))
                    {
                        newrolelist.Add(new IdentityRole(role));
                    }
                }
                context.Roles.AddRange(newrolelist);
                context.SaveChanges();
            }
        }
    }
}