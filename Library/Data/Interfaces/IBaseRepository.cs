namespace Library.Data.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task<bool> Delete(T entity);
    Task<bool> Add(T entity);
    Task<bool> Update(T entity);
}