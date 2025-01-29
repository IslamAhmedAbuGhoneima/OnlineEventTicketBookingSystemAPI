namespace Shared.DTOS;

public class EventDto
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Location { get; set; } = null!;

    public decimal TicketPrice { get; set; }

    public DateTime DateTime { get; set; }

    public int TotalSeats { get; set; }

    public string OrganizerId { get; set; } = null!;
}

