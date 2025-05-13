using EventEaseApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Threading.Tasks;

namespace EventEaseApplication.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EventController(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IActionResult> Index()
        {
            var events = await _context.Event.Include(e => e.Venue).ToListAsync();
            return View(events);
        }

        public IActionResult Create()
        {
            var venues = _context.Venue.OrderBy(e => e.VenueName).ToList();
            if (!venues.Any())
            {
                return RedirectToAction("Create", "Venue");
            }

            ViewBag.Venue = new SelectList(venues, "Venue_ID", "VenueName");
            return View(new Event());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Venue_ID,EventName,EventDate,Description,ImageURL")] Event events)
        {

            try
            {
                // Check if venue exists
                var venue = await _context.Venue.FindAsync(events.Venue_ID);
                if (venue == null)
                {
                    ModelState.AddModelError("Venue_ID", "Invalid venue selected");
                }

                if (ModelState.IsValid)
                {
                    // Set default image URL if not provided
                    if (string.IsNullOrEmpty(events.ImageURL))
                    {
                        events.ImageURL = "https://via.placeholder.com/300x200?text=Event+Image";
                    }

                    _context.Add(events);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " + ex.Message);
            }

            ViewBag.Venue = new SelectList(_context.Venue.OrderBy(v => v.VenueName), "Venue_ID", "VenueName", events.Venue_ID);
            return View(events);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
               return NotFound();
            }

            var events = await _context.Event.Include(e => e.Venue).Include(e => e.Booking).FirstOrDefaultAsync(m => m.Event_ID == id);

            if (events == null)
            {
               return NotFound();
            }
               return View(events);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var events = await _context.Event
                .Include(e => e.Booking)
                .FirstOrDefaultAsync(e => e.Event_ID == id);

            if (events == null) return NotFound();

            if (events.Booking.Any())
            {
                TempData["ErrorMessage"] =
                    "Cannot delete this event because there are active bookings.";
                return RedirectToAction(nameof(Index));
            }

            return View(events);
        }

        [HttpPost]

        public async Task<IActionResult> Delete(int id)
        {
            var events = await _context.Event.FindAsync(id);
            _context.Event.Remove(events);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExist(int id)
        {
            return _context.Event.Any(e => e.Event_ID == id);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Event.FindAsync(id);
            if (events == null)
            {
                return NotFound();
            }
            ViewBag.Venue = new SelectList(_context.Venue.OrderBy(v => v.VenueName), "Venue_ID", "VenueName", events.Venue_ID);
            return View(events);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Event_ID,Venue_ID,EventName,EventDate,Description,ImageURL")] Event events)
        {
            if (id != events.Event_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(events);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExist(events.Event_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewBag.Venue = new SelectList(_context.Venue.OrderBy(v => v.VenueName), "Venue_ID", "VenueName", events.Venue_ID);
            return View(events);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // 1) Prevent delete if there are bookings for this event
            bool hasBookings = await _context.Booking
                .AnyAsync(b => b.Event_ID == id);
            if (hasBookings)
            {
                TempData["ErrorMessage"] = "Cannot delete this event—it has existing bookings.";
                return RedirectToAction(nameof(Index));
            }

            // 2) Safe to delete
            var events = await _context.Event.FindAsync(id);
            _context.Event.Remove(events);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
