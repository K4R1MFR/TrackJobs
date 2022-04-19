#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrackJobs.Areas.Member.Data;
using TrackJobs.Data;

namespace TrackJobs.Areas.Member.Controllers
{
    [Area("Member")]
    public class CommunicationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommunicationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Member/Communication
        // id here is JobOfferId, title is JobOffer JobTitle
        public async Task<IActionResult> Index(Guid? id, string title)
        {
            if (id is null)
            {
                return NotFound();
            }

            ViewBag.jobOfferId = id.Value;
            ViewBag.jobTitle = title;

            var applicationDbContext = _context.Communications
                .Include(c => c.JobOffer)
                .Where(c => c.JobOfferId == id)
                .OrderByDescending(c => c.Date);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Member/Communication/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var communication = await _context.Communications
                .Include(c => c.JobOffer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (communication == null)
            {
                return NotFound();
            }

            return View(communication);
        }

        // GET: Member/Communication/Create
        // id here is JobOfferId
        public IActionResult Create(Guid? id)
        {
            if(id is null)
            {
                return NotFound();
            }

            var m = new Models.Communication.Create
            {
                JobOfferId = id.Value,
                Date = DateTime.Parse(DateTime.Now.ToString("f"))
            };

            return View(m);
        }

        // POST: Member/Communication/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobOfferId,Date,CommunicationType,Title,Description")] Models.Communication.Create m)
        {
            if (ModelState.IsValid)
            {
                var communication = new Communication
                {
                    JobOfferId = m.JobOfferId,
                    Date = m.Date,
                    CommunicationType = m.CommunicationType,
                    Title = m.Title,
                    Description = m.Description
                };

                _context.Add(communication);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Communication", new { id = m.JobOfferId });
            }

            return View(m);
        }

        // GET: Member/Communication/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var communication = await _context.Communications.FindAsync(id);
            if (communication == null)
            {
                return NotFound();
            }

            var m = new Models.Communication.Edit
            {
                Id = communication.Id,
                JobOfferId = communication.JobOfferId,
                Date = communication.Date,
                CommunicationType = communication.CommunicationType,
                Title = communication.Title,
                Description = communication.Description
            };

            return View(m);
        }

        // POST: Member/Communication/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JobOfferId,Date,CommunicationType,Title,Description")] Models.Communication.Edit m)
        {
            if (id != m.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var communication = new Communication
                    {
                        Id = m.Id,
                        JobOfferId = m.JobOfferId,
                        Date = m.Date,
                        CommunicationType = m.CommunicationType,
                        Title = m.Title,
                        Description = m.Description

                    };

                    _context.Update(communication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommunicationExists(m.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var jobOffer = _context.JobOffers.Where(j => j.GuId == m.JobOfferId).FirstOrDefault();

                if(jobOffer is null)
                {
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Index", "Communication", new { id = m.JobOfferId, title = jobOffer.OfferTitle });
            }

            return View(m);
        }

        // GET: Member/Communication/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var communication = await _context.Communications
                .Include(c => c.JobOffer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (communication == null)
            {
                return NotFound();
            }

            return View(communication);
        }

        // POST: Member/Communication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var communication = await _context.Communications.FindAsync(id);
            _context.Communications.Remove(communication);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Communication", new { id = communication.JobOfferId });
        }

        private bool CommunicationExists(int id)
        {
            return _context.Communications.Any(e => e.Id == id);
        }
    }
}
