using Contracts;
using Entities.Models;

namespace Repository;
public class BookRepository : BaseRepository<Book>, IBookRepository
{
    public BookRepository(AppDbContext context) 
        : base(context) { }
    
}
