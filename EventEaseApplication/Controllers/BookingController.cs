using EventEaseApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEaseApplication.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BookingController(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Booking.Include(b => b.Event).Include(b => b.Venue).ToListAsync();
            return View(bookings);
        }

        public IActionResult Create()
        {
            // Check if venues and events exist before allowing booking creation
            if (!_context.Venue.Any() || !_context.Event.Any())
            {
                TempData["ErrorMessage"] = "You need to create venues and events before creating bookings.";
                return RedirectToAction("Index");
            }

            ViewBag.Event = new SelectList(_context.Event, "Event_ID", "EventName");
            ViewBag.Venue = new SelectList(_context.Venue, "Venue_ID", "VenueName");
            return View(new Booking());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            bool conflict = await _context.Booking
       .AnyAsync(b =>
           b.Venue_ID == booking.Venue_ID &&
           b.BookingDate == booking.BookingDate
       );

            if (conflict)
            {
                ModelState.AddModelError(
                    nameof(booking.BookingDate),
                    "That venue is already booked at this date/time."
                );
            }

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If we got this far, something failed, redisplay form
            ViewBag.Event = new SelectList(_context.Event, "Event_ID", "EventName", booking.Event_ID);
            ViewBag.Venue = new SelectList(_context.Venue, "Venue_ID", "VenueName", booking.Venue_ID);
            return View(booking);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.Include(b => b.Event).Include(b => b.Venue).FirstOrDefaultAsync(b => b.Booking_ID == id);

            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            ViewBag.Event = new SelectList(_context.Event, "Event_ID", "EventName", booking.Event_ID);
            ViewBag.Venue = new SelectList(_context.Venue, "Venue_ID", "VenueName", booking.Venue_ID);
            return View(booking);
        }

        private bool BookingExist(int id)
        {
            return _context.Booking.Any(a => a.Booking_ID == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,VenueId,EventId,BookingDate")] Booking booking)
        {
            if (id != booking.Booking_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExist(booking.Booking_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.Event = new SelectList(_context.Event, "Event_ID", "EventName", booking.Event_ID);
            ViewBag.Venue = new SelectList(_context.Venue, "Venue_ID", "VenueName", booking.Venue_ID);
            return View(booking);
        }
    }
}

