using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEaseApplication.Models
{
    [Table("Booking")]
    public class Booking
    {
        [Key] 
        [Column("Booking_ID")]
        public int Booking_ID { get; set; }

        [Column("Event_ID")]
        public int Event_ID { get; set; }

        [Column("Venue_ID")]
        public int Venue_ID { get; set; }
        public DateOnly BookingDate { get; set; }

        [ForeignKey(nameof(Event_ID))]
        public Event Event { get; set; }

        [ForeignKey(nameof(Venue_ID))]
        public Venue Venue { get; set; }

    }
}
