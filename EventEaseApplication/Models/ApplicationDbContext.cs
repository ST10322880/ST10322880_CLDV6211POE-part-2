using Microsoft.EntityFrameworkCore;

namespace EventEaseApplication.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Event> Event { get; set; }
        public DbSet<Venue> Venue { get; set; }
        public DbSet<Booking> Booking { get; set; }







        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Venue>()
            .HasMany(v => v.Event)  // ← Connect to the new list
            .WithOne(e => e.Venue)
            .HasForeignKey(e => e.Venue_ID);

            // Configure relationships
            modelBuilder.Entity<Event>()
                .ToTable("Event")
                .HasOne(e => e.Venue)
                .WithMany(v => v.Event)
                .HasForeignKey(e => e.Venue_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithMany(e => e.Booking)
                .HasForeignKey(b => b.Event_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Venue)
                .WithMany(v => v.Booking)
                .HasForeignKey(b => b.Venue_ID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EnhancedBooking>(eb =>
            {
                eb.HasNoKey();                   // it’s a view, not a table
                eb.ToView("EnhancedBooking");  // match your SQL view name
            });




        }
    }

}
