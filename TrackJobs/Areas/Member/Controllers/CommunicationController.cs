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
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Communications.Include(c => c.JobOffer);
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
        public IActionResult Create(int id)
        {
            var m = new Models.Communication.Create
            {
                JobOfferId = id,
                Date = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy HH:mm"))
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
                return RedirectToAction(nameof(Index));
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
            ViewData["JobOfferId"] = new SelectList(_context.JobOffers, "Id", "Id", communication.JobOfferId);
            return View(communication);
        }

        // POST: Member/Communication/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JobOfferId,Date,CommunicationType,Title,Description")] Communication communication)
        {
            if (id != communication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(communication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommunicationExists(communication.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["JobOfferId"] = new SelectList(_context.JobOffers, "Id", "Id", communication.JobOfferId);
            return View(communication);
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
            return RedirectToAction(nameof(Index));
        }

        private bool CommunicationExists(int id)
        {
            return _context.Communications.Any(e => e.Id == id);
        }
    }
}
