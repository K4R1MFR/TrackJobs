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
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Member/Contact
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Contacts.Include(c => c.JobOffer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Member/Contact/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.JobOffer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Member/Contact/Create
        // id here is JobOfferId
        public IActionResult Create(int id)
        {
            var m = new Models.Contact.Create
            {
                JobOfferId = id
            };

            return View(m);
        }

        // POST: Member/Contact/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobOfferId,FirstName,LastName,Title,Email,PhoneNumber")] Models.Contact.Create m)
        {
            if (ModelState.IsValid)
            {
                var contact = new Contact
                {
                    JobOfferId = m.JobOfferId,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Title = m.Title,
                    Email = m.Email,
                    PhoneNumber = m.PhoneNumber
                };

                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return View(m);
        }

        // GET: Member/Contact/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            var m = new Models.Contact.Edit
            {
                Id = contact.Id,
                JobOfferId = contact.JobOfferId,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Title = contact.Title,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber
            };


            return View(m);
        }

        // POST: Member/Contact/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JobOfferId,FirstName,LastName,Title,Email,PhoneNumber")] Models.Contact.Edit m)
        {
            if (id != m.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var contact = new Contact
                {
                    Id = m.Id,
                    JobOfferId = m.JobOfferId,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Title = m.Title,
                    Email = m.Email,
                    PhoneNumber = m.PhoneNumber
                };

                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
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

            return View(m);
        }

        // GET: Member/Contact/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.JobOffer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Member/Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}
