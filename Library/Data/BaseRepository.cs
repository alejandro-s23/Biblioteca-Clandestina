using Library.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Data;

public abstract class BaseRepository<T>(LibraryContext context) : IBaseRepository<T> where T : class
{
    private DbSet<T> DbSet => context.Set<T>();
    
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
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
}