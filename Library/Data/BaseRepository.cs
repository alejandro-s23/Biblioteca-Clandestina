using Library.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public abstract class BaseRepository<T>(LibraryContext context) : IBaseRepository<T> where T : class
{
    protected DbSet<T> DbSet => context.Set<T>();
    
    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await DbSet.FindAsync(id) != null;
    }

    public async Task<IEnumerable<T>> GetEntitiesAsync(int size = 0)
    {
        return size == 0 ? await DbSet.ToListAsync() : await DbSet.Take(size).ToListAsync();
    }
    
    public async Task<List<T>> GetEntitiesListAsync(int size = 0)
    {
        return size == 0 ? await DbSet.ToListAsync() : await DbSet.Take(size).ToListAsync();
    }

    public async Task<bool> Delete(T entity)
    {
        try
        {
            DbSet.Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        
    }

    public async Task<bool> Add(T entity)
    {
        try
        {
            DbSet.Add(entity);
            await context.SaveChangesAsync();
            return true;
        }catch(Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> SaveAsync()
    {
        try
        {
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> Update(T entity)
    {
        try
        {
            DbSet.Update(entity);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<int> CountAsync()
    {
        return await DbSet.CountAsync();
    }
}