namespace Library.Data.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<List<T>> GetEntitiesListAsync(int size = 0);
    Task<IEnumerable<T>> GetEntitiesAsync(int size = 0);
    Task<bool> Delete(T entity);
    Task<bool> Add(T entity);
    Task<bool> Update(T entity);
    Task<int> CountAsync();
    Task<bool> SaveAsync();
}