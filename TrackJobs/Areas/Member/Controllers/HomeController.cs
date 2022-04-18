using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TrackJobs.Data;
using TrackJobs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using TrackJobs.Areas.Member.Data;

namespace TrackJobs.Areas.Member.Controllers
{
    [Area("Member"), Authorize(Roles = "Member, Admin, Demo")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string orderBy)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.user = user;

            List<JobOffer> jobOffers = new List<JobOffer>();

            if (orderBy is null || orderBy == "activity")
            {
                jobOffers = await _context.JobOffers
                    .Where(j => j.UserId == user.Id)
                    .Where(j => j.IsSoftDeleted == false)
                    .Where(j => j.IsClosed == false)
                    .Where(j => j.IsRejected == false)
                    .Include(j => j.Source)
                    .Include(j => j.Contacts)
                    .Include(j => j.Communications)
                    .OrderByDescending(j => j.Communications.OrderBy(c => c.Date).LastOrDefault().Date)
                    .ToListAsync();
            }
            else if (orderBy == "favorite")
            {
                jobOffers = await _context.JobOffers
                    .Where(j => j.UserId == user.Id)
                    .Where(j => j.IsSoftDeleted == false)
                    .Where(j => j.IsClosed == false)
                    .Where(j => j.IsRejected == false)
                    .Include(j => j.Source)
                    .Include(j => j.Contacts)
                    .Include(j => j.Communications)
                    .OrderByDescending(j => j.IsFavorite)
                    .ToListAsync();
            }
            else if (orderBy == "applied")
            {
                jobOffers = await _context.JobOffers
                    .Where(j => j.UserId == user.Id)
                    .Where(j => j.IsSoftDeleted == false)
                    .Where(j => j.IsClosed == false)
                    .Where(j => j.IsRejected == false)
                    .Include(j => j.Source)
                    .Include(j => j.Contacts)
                    .Include(j => j.Communications)
                    .OrderByDescending(j => j.AppliedOn)
                    .ToListAsync();

            }

            ViewBag.jobOffers = jobOffers;

            return View(jobOffers);
        }
        public async Task<IActionResult> ChangeOrder(string order)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.user = user;

            List<JobOffer> jobOffers = new List<JobOffer>();

            if(order is null)
            {
                return NotFound();
            } else if(order == "favorite")
            {
                jobOffers = _context.JobOffers
                    .Where(j => j.UserId == user.Id)
                    .Where(j => j.IsSoftDeleted == false)
                    .Include(j => j.Source)
                    .Include(j => j.Contacts)
                    .Include(j => j.Communications)
                    .OrderByDescending(j => j.IsFavorite)
                    .ToList();
            }
            else if (order == "applied")
            {
                jobOffers = _context.JobOffers
                    .Where(j => j.UserId == user.Id)
                    .Where(j => j.IsSoftDeleted == false)
                    .Include(j => j.Source)
                    .Include(j => j.Contacts)
                    .Include(j => j.Communications)
                    .OrderByDescending(j => j.AppliedOn)
                    .ToList();

            }
            else if(order == "activity")
            {
                jobOffers = _context.JobOffers
                    .Where(j => j.UserId == user.Id)
                    .Where(j => j.IsSoftDeleted == false)
                    .Include(j => j.Source)
                    .Include(j => j.Contacts)
                    .Include(j => j.Communications)
                    .OrderByDescending(j => j.Communications.OrderBy(c => c.Date).LastOrDefault().Date)
                    .ToList();
            }


            ViewBag.jobOffers = jobOffers;

            return View(jobOffers);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}