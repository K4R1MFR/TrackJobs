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
    [Area("Member"), Authorize(Roles = "Member, Admin")]

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
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return NotFound();
            }

            var applicationDbContext = _context.JobOffers
                .Include(j => j.Source)
                .Where(j => j.UserId == userId)
                .Where(j => j.IsSoftDeleted == false);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: JobOffer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOffer = await _context.JobOffers
                .Include(j => j.Source)
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Create([Bind("UserId, AppliedOn, CompanyName, OfferTitle, SourceId, LinkToOffer, " +
            "HasSentResume, HasSentCoverLetter, Salary, IsFavorite, Perks, " +
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
                    SourceId = m.SourceId,
                    LinkToOffer = m.LinkToOffer,
                    HasSentResume = m.HasSentResume,
                    HasSentCoverLetter = m.HasSentCoverLetter,
                    Salary = m.Salary,
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
        public async Task<IActionResult> Edit(int? id)
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
                Id = jobOffer.Id,
                UserId = jobOffer.UserId,
                AppliedOn = jobOffer.AppliedOn,
                CompanyName = jobOffer.CompanyName,
                OfferTitle = jobOffer.OfferTitle,
                SourceId = jobOffer.SourceId,
                LinkToOffer = jobOffer.LinkToOffer,
                HasSentResume = jobOffer.HasSentResume,
                HasSentCoverLetter = jobOffer.HasSentCoverLetter,
                Salary = jobOffer.Salary,
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
        public async Task<IActionResult> Edit(int id, [Bind("Id, UserId, AppliedOn, CompanyName, OfferTitle, SourceId, LinkToOffer, " +
            "HasSentResume, HasSentCoverLetter, Salary, IsFavorite, Perks, " +
            "Pros, Cons, StreetNumber, StreetName, City, Postcode, State")] Models.JobOffer.Edit m)
        {
            if (id != m.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var editedJobOffer = new JobOffer
                    {
                        Id = m.Id,
                        UserId = m.UserId,
                        AppliedOn = m.AppliedOn,
                        CompanyName = m.CompanyName,
                        OfferTitle = m.OfferTitle,
                        SourceId = m.SourceId,
                        LinkToOffer = m.LinkToOffer,
                        HasSentResume = m.HasSentResume,
                        HasSentCoverLetter = m.HasSentCoverLetter,
                        Salary = m.Salary,
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
                    if (!JobOfferExists(m.Id))
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

        // GET: JobOffer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOffer = await _context.JobOffers
                .Include(j => j.Source)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobOffer == null)
            {
                return NotFound();
            }

            return View(jobOffer);
        }

        // POST: JobOffer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobOffer = await _context.JobOffers.FindAsync(id);
            jobOffer.IsSoftDeleted = true;
            _context.JobOffers.Update(jobOffer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool JobOfferExists(int id)
        {
            return _context.JobOffers.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ChangeFavorite(int id)
        {
            var jobOffer = _context.JobOffers.Where(j => j.Id == id).FirstOrDefault();

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
