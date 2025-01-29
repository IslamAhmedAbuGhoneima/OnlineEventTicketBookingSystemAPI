using Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly AppDbContext _context;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity)
    {
        await _context.AddAsync(entity);
    }

    public void Delete(T entity)
    {
         _context.Remove(entity);
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, string? includes, bool trackChanges)
    {
        IQueryable<T> query = _context.Set<T>();
        if (includes != null)
        {
            string[] includeProperties = includes.Split(',');
            foreach(string includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }

        return trackChanges ?
            query.Where(expression) :
            query.AsNoTracking().Where(expression);
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _context.FindAsync<T>(id);
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public void Update(T entity)
    {
        _context.Update(entity);
    }
}
