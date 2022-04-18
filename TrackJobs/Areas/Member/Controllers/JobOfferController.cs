#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrackJobs.Areas.Member.Data;
using TrackJobs.Data;

namespace TrackJobs.Areas.Member.Controllers
{
    [Area("Member"), Authorize(Roles = "Member, Admin, Demo")]

    public class JobOfferController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public JobOfferController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: JobOffer
        public async Task<IActionResult> Index(string orderBy)
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return NotFound();
            }

            List<JobOffer> jobOffers = new List<JobOffer>();

            if (orderBy is null || orderBy == "favorite")
            {
                jobOffers = await _context.JobOffers
                .Include(j => j.Source)
                .Where(j => j.UserId == userId)
                .Where(j => j.IsSoftDeleted == false)
                .OrderByDescending(j => j.IsFavorite)
                .ToListAsync();
            } else if(orderBy == "activity")
            {
                jobOffers = await _context.JobOffers
                .Include(j => j.Source)
                .Where(j => j.UserId == userId)
                .Where(j => j.IsSoftDeleted == false)
                .OrderByDescending(j => j.Communications.OrderBy(c => c.Date).LastOrDefault().Date)
                .ToListAsync();
            } else if(orderBy == "applied")
            {
                jobOffers = await _context.JobOffers
                .Include(j => j.Source)
                .Where(j => j.UserId == userId)
                .Where(j => j.IsSoftDeleted == false)
                .OrderByDescending(j => j.AppliedOn)
                .ToListAsync();
            }

            return View(jobOffers);
        }

        // GET: JobOffer/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOffer = await _context.JobOffers
                .Include(j => j.Source)
                .Include(j => j.Communications)
                .Include(j => j.Contacts)
                .FirstOrDefaultAsync(m => m.GuId == id);
            if (jobOffer == null)
            {
                return NotFound();
            }

            ViewBag.contacts = jobOffer.Contacts.ToList();
            ViewBag.communication = jobOffer.Communications.OrderBy(c => c.Date).ToList();

            return View(jobOffer);
        }

        // GET: JobOffer/Create
        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return NotFound();
            }

            var m = new Models.JobOffer.Create
            {
                UserId = userId,
                AppliedOn = DateTime.Parse(DateTime.Now.ToString("f"))
            };

            ViewData["SourceId"] = new SelectList(_context.Sources, "Id", "Name");
            return View(m);
        }

        // POST: JobOffer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId, AppliedOn, CompanyName, OfferTitle, Description, SourceId, LinkToOffer, " +
            "HasSentResume, HasSentCoverLetter, Salary, IsWFHAvailable, IsFavorite, Perks, " +
            "Pros, Cons, StreetNumber, StreetName, City, Postcode, State")] Models.JobOffer.Create m)
        {
            if (ModelState.IsValid)
            {
                var jobOffer = new Data.JobOffer
                {
                    UserId = m.UserId,
                    AppliedOn = m.AppliedOn,
                    CompanyName = m.CompanyName,
                    OfferTitle = m.OfferTitle,
                    Description = m.Description?.Replace("\n", "<br/>"),
                    SourceId = m.SourceId,
                    LinkToOffer = m.LinkToOffer,
                    HasSentResume = m.HasSentResume,
                    HasSentCoverLetter = m.HasSentCoverLetter,
                    Salary = m.Salary,
                    IsWFHAvailable = m.IsWFHAvailable,
                    IsFavorite = m.IsFavorite,
                    Perks = m.Perks,
                    Pros = m.Pros,
                    Cons = m.Cons,
                    StreetNumber = m.StreetNumber,
                    StreetName = m.StreetName,
                    City = m.City,
                    Postcode = m.Postcode,
                    State = m.State
                };

                _context.Add(jobOffer);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            ViewData["SourceId"] = new SelectList(_context.Sources, "Id", "Name", m.SourceId);
            return View(m);
        }

        // GET: JobOffer/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOffer = await _context.JobOffers.FindAsync(id);
            if (jobOffer == null)
            {
                return NotFound();
            }
            var m = new Models.JobOffer.Edit
            {
                GuId = jobOffer.GuId,
                UserId = jobOffer.UserId,
                AppliedOn = jobOffer.AppliedOn,
                CompanyName = jobOffer.CompanyName,
                OfferTitle = jobOffer.OfferTitle,
                Description = jobOffer.Description?.Replace("<br/>", "\r\n"),
                SourceId = jobOffer.SourceId,
                LinkToOffer = jobOffer.LinkToOffer,
                HasSentResume = jobOffer.HasSentResume,
                HasSentCoverLetter = jobOffer.HasSentCoverLetter,
                Salary = jobOffer.Salary,
                IsWFHAvailable = jobOffer.IsWFHAvailable,
                IsFavorite = jobOffer.IsFavorite,
                Perks = jobOffer.Perks,
                Pros = jobOffer.Pros,
                Cons = jobOffer.Cons,
                StreetNumber = jobOffer.StreetNumber,
                StreetName = jobOffer.StreetName,
                City = jobOffer.City,
                Postcode = jobOffer.Postcode,
                State = jobOffer.State


            };

            ViewData["SourceId"] = new SelectList(_context.Sources, "Id", "Name", jobOffer.SourceId);
            return View(m);
        }

        // POST: JobOffer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GuId, UserId, AppliedOn, CompanyName, OfferTitle, Description, SourceId, LinkToOffer, " +
            "HasSentResume, HasSentCoverLetter, Salary, IsWFHAvailable, IsFavorite, Perks, " +
            "Pros, Cons, StreetNumber, StreetName, City, Postcode, State")] Models.JobOffer.Edit m)
        {
            if (id != m.GuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var editedJobOffer = new JobOffer
                    {
                        GuId = m.GuId,
                        UserId = m.UserId,
                        AppliedOn = m.AppliedOn,
                        CompanyName = m.CompanyName,
                        OfferTitle = m.OfferTitle,
                        Description = m.Description?.Replace("\r\n", "<br/>"),
                        SourceId = m.SourceId,
                        LinkToOffer = m.LinkToOffer,
                        HasSentResume = m.HasSentResume,
                        HasSentCoverLetter = m.HasSentCoverLetter,
                        Salary = m.Salary,
                        IsWFHAvailable = m.IsWFHAvailable,
                        IsFavorite = m.IsFavorite,
                        Perks = m.Perks,
                        Pros = m.Pros,
                        Cons = m.Cons,
                        StreetNumber = m.StreetNumber,
                        StreetName = m.StreetName,
                        City = m.City,
                        Postcode = m.Postcode,
                        State = m.State

                    };

                    _context.Update(editedJobOffer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobOfferExists(m.GuId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            ViewData["SourceId"] = new SelectList(_context.Sources, "Id", "Name", m.SourceId);
            return View(m);
        }

        // GET: JobOffer/Close/5
        public async Task<IActionResult> Close(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOffer = await _context.JobOffers
                .Include(j => j.Source)
                .FirstOrDefaultAsync(m => m.GuId == id);
            if (jobOffer == null)
            {
                return NotFound();
            }

            return View(jobOffer);
        }

        // POST: JobOffer/Close/5
        [HttpPost, ActionName("Close")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseConfirmed(Guid id)
        {
            var jobOffer = await _context.JobOffers.FindAsync(id);
            jobOffer.IsClosed = true;
            _context.JobOffers.Update(jobOffer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: JobOffer/Reject/5
        public async Task<IActionResult> Reject(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOffer = await _context.JobOffers
                .Include(j => j.Source)
                .FirstOrDefaultAsync(m => m.GuId == id);
            if (jobOffer == null)
            {
                return NotFound();
            }

            return View(jobOffer);
        }

        // POST: JobOffer/Reject/5
        [HttpPost, ActionName("Reject")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectConfirmed(Guid id, string rejectionFeedback)
        {
            var jobOffer = await _context.JobOffers.FindAsync(id);
            jobOffer.IsRejected = true;
            jobOffer.RejectionFeedback = rejectionFeedback;
            _context.JobOffers.Update(jobOffer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }


        // GET: JobOffer/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOffer = await _context.JobOffers
                .Include(j => j.Source)
                .FirstOrDefaultAsync(m => m.GuId == id);
            if (jobOffer == null)
            {
                return NotFound();
            }

            return View(jobOffer);
        }

        // POST: JobOffer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var jobOffer = await _context.JobOffers.FindAsync(id);
            jobOffer.IsSoftDeleted = true;
            _context.JobOffers.Update(jobOffer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool JobOfferExists(Guid id)
        {
            return _context.JobOffers.Any(e => e.GuId == id);
        }

        public async Task<IActionResult> ChangeFavorite(Guid id)
        {
            var jobOffer = _context.JobOffers.Where(j => j.GuId == id).FirstOrDefault();

            if(jobOffer.IsFavorite == false)
            {
                jobOffer.IsFavorite = true;
            } else
            {
                jobOffer.IsFavorite = false;
            }
            _context.Update(jobOffer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
