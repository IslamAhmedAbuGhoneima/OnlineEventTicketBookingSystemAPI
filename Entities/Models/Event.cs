using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Event
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Location { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public decimal TicketPrice { get; set; }

    public int TotalSeats { get; set; }

    public int AvailableSeats { get; set; }

    // Navigation property for the organizer
    [ForeignKey("Organizer")]
    public string OrganizerId { get; set; } = null!; // Foreign key to ApplicationUser
    public ApplicationUser? Organizer { get; set; } // Reference to the ApplicationUser who organizes the event

    ICollection<Book> Tickets { get; set; } = [];
}
