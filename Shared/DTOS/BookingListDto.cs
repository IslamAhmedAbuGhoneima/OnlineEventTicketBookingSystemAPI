namespace Shared.DTOS;

public class BookingListDto
{
    public Guid EventId { get; set; }

    public string EventName { get; set; } = null!;

    public DateTime EventDate { get; set; }

    public decimal TicketPrice { get; set; }

    public int ReservedSeats { get; set; }


}
