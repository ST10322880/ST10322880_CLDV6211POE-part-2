using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEaseApplication.Models
{
    [Table("Venue")]
    public class Venue
    {
        [Key]
        [Column("Venue_ID")]
        public int Venue_ID { get; set; }
        public string VenueName { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string ImageURL { get; set; } = "https://placehold.co/600x400";
        public List<Booking> Booking { get; set; } = new();
        public List<Event> Event { get; set; } = new();
    }
}
