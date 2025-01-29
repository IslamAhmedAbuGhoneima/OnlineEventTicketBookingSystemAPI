using System.Linq.Expressions;

namespace Contracts;

public interface IBaseRepository<T> 
{
    Task<T> GetByIdAsync(Guid id);

    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, string? includes, bool trackChanges);

    Task<IReadOnlyList<T>> ListAllAsync();

    Task AddAsync(T entity);

    void Update(T entity);

    void Delete(T entity);
}
