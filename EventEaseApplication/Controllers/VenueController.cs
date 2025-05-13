using EventEaseApplication.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEaseApplication.Controllers
{
    public class VenueController : Controller
    {
        private readonly ApplicationDbContext _context;
        public VenueController(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IActionResult> Index()
        {
            var venues = await _context.Venue.ToListAsync();
            return View(venues);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Venue venues)
        {
            if (!ModelState.IsValid)
            {
                // return view with validation errors
                return View(venues);
            }

            try
            {
                _context.Add(venues);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                // optionally log ex
                return View(venues);
            }
        }

        public async Task<IActionResult> Details(int? id)
        { 
            var venues = await _context.Venue.FirstOrDefaultAsync(v => v.Venue_ID == id);
            if (venues == null)
            { 
                return NotFound();  
            }
            return View(venues);
        }

        // GET: /Venue/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var venues = await _context.Venue.Include(v => v.Booking)  .FirstOrDefaultAsync(v => v.Venue_ID == id);

            if (venues == null) return NotFound();

            // If bookings exist, show a warning instead of the delete confirmation
            if (venues.Booking.Any())
            {
                TempData["ErrorMessage"] =
                    "Cannot delete this venue because there are active bookings.";
                return RedirectToAction(nameof(Index));
            }

            return View(venues);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var venues = await _context.Venue.FindAsync(id);
            {
                _context.Venue.Remove(venues);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }


        private bool VenueExist(int id)
        {
            return _context.Venue.Any(a => a.Venue_ID == id);
        }

        
        

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venues = await _context.Venue.FindAsync(id);
            if (venues == null)
            {
                return NotFound();
            }
            return View(venues);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(int id, Venue venues)
        {
            if (id != venues.Venue_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venues);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExist(venues.Venue_ID))
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
            return View(venues);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // 1) Check for active bookings
            bool hasBookings = await _context.Booking
                .AnyAsync(b => b.Venue_ID == id);
            if (hasBookings)
            {
                TempData["ErrorMessage"] = "Cannot delete this venue, it has existing bookings.";
                return RedirectToAction(nameof(Index));
            }

            // 2) Safe to delete
            var venues = await _context.Venue.FindAsync(id);
            _context.Venue.Remove(venues);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
        


    }

}
