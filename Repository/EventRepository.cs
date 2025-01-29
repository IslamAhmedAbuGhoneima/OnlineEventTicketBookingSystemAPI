using Contracts;
using Entities.Models;

namespace Repository;
public class EventRepository : BaseRepository<Event>, IEventRepository
{
    public EventRepository(AppDbContext context) 
        : base(context) { }
    
}
