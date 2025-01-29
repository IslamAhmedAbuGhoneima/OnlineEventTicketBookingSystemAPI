using Microsoft.AspNetCore.Identity;

namespace Entities.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<Book> Bookings { get; set; } = [];
    public ICollection<Event>? OrganizedEvents { get; set; } // If Organizer
}
