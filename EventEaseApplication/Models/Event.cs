using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventEaseApplication.Models
{
    [Table("Event")]
    public class Event
    {
        [Key]
        [Column("Event_ID")]
        public int Event_ID { get; set; }

        [Required]
        [Display(Name = "Venue")]
        [ForeignKey("Venue")]
        public int Venue_ID { get; set; }

        [Required]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [Required]
        [Display(Name = "Event Date")]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Required]
        public string Description { get; set; }

        public string ImageURL { get; set; } = "https://placehold.co/600x400";

        [ForeignKey(nameof(Venue_ID))]
        public Venue Venue { get; set; }
        public List<Booking> Booking { get; set; } = new();
       

    }

}

