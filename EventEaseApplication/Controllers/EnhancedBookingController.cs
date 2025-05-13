using EventEaseApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEaseApplication.Controllers
{
    public class EnhancedBookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EnhancedBookingController(ApplicationDbContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Index(string filter)
        {
            var items = await _context.Booking.Include(b => b.Event).Include(b => b.Venue).Select(b => new EnhancedBooking
                {
                    Booking_ID = b.Booking_ID,
                    BookingDate = b.BookingDate,
                    EventName = b.Event.EventName,
                    EventDate = b.Event.EventDate,
                    Description = b.Event.Description,
                    VenueName = b.Venue.VenueName,
                    Capacity = b.Venue.Capacity
                })
                .ToListAsync();

            return View(items);
        }
    }
}
