namespace Shared.DTOS;

public class BookingForCreationDto 
{
    public int SeatsNumber { get; set; }

    public DateTime BookingDate { get; set; }

    public string UserId { get; set; } = null!;

    public Guid EventId { get; set; }

}
