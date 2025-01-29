using Contracts;

namespace Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IBookRepository _booking;
    private readonly IEventRepository _events;
    private readonly IUserRepository _user;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        _booking = new BookRepository(_context);
        _events = new EventRepository(_context);
        _user = new UserRepository(_context);
    }

    public IEventRepository Events => _events;

    public IBookRepository Booking => _booking;

    public IUserRepository Users => _user;

    public async Task SaveAsync()
    {
       await _context.SaveChangesAsync();
    }
}
