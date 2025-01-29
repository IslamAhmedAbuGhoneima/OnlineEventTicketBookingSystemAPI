using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Book
{
    public Guid Id { get; set; }

    public int SeatsNumber { get; set; }

    public DateTime BookingDate { get; set; }

    public decimal TotalPrice { get; set; }

    public ApplicationUser? User { get; set; }

    [ForeignKey("User")]
    public string UserId { get; set; } = null!;

    public Event? Event { get; set; }

    [ForeignKey("Event")]
    public Guid EventId { get; set; }

}
