using Contracts;
using Entities.Models;

namespace Repository;

internal class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
{
    public UserRepository(AppDbContext context) 
        : base(context) { }
    
}
