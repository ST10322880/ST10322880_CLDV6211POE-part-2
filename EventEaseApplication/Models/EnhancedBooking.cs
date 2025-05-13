namespace EventEaseApplication.Models
{
    public class EnhancedBooking
    {
        public int Booking_ID { get; set; }
        public DateOnly BookingDate { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public string VenueName { get; set; }
        public int Capacity { get; set; }
    }
}
