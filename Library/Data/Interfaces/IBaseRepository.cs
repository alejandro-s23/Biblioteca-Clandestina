namespace Library.Data.Interfaces;

public interface IBaseRepository<in T> where T : class
{
    Task<T?> GetByIdAsync<T>(Guid id) where T : class;
    Task<List<T>> GetAllAsync<T>() where T : class;
    Task<bool> Delete(T entity);
    Task<bool> Add(T entity);
    Task<bool> Update(T entity);
}