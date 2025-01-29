namespace Contracts;

public interface IUnitOfWork
{
    IBookRepository Booking { get; }

    IEventRepository Events { get; }

    IUserRepository Users { get; }

    Task SaveAsync();
}
