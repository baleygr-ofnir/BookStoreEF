using System.Linq.Expressions;
using BookStoreEF.Data;
using BookStoreEF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookStoreEF.Management;

public abstract class GenericRepository<T>
    : IRepository<T> where T : class
{
    protected BookStoreContext Context;
    
    public GenericRepository(BookStoreContext context)
    {
        Context = context;
    }
    
    public virtual async Task<T> Add(T entity)
    {
        await Context.AddAsync(entity);
        return entity;
    }

    public virtual T Update(T entity)
    {
        Context.Update(entity);
        return entity;
    }

    public virtual async Task<T> Get(int id)
    {
        return await Context.FindAsync<T>(id);
    }
    
    public virtual async Task<T> Get(string id)
    {
        return await Context.FindAsync<T>(id);
    }

    public virtual async Task<IEnumerable<T>> All()
    {
        return await Context.Set<T>()
            .AsNoTracking()
            .ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
    {
        return await Context.Set<T>()
            .AsQueryable()
            .Where(predicate)
            .ToListAsync();
    }

    public virtual async Task<T?> FindOne(Expression<Func<T, bool>> predicate)
    {
        return await Context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public async Task<bool> Delete(int id)
    {
        T entity = await Get(id);
        if (entity == null) return false;

        Context.Set<T>().Remove(entity);
        return true;
    }
    public async Task<bool> Delete(string id)
    {
        T entity = await Get(id);
        if (entity == null) return false;

        Context.Set<T>().Remove(entity);
        return true;
    }

    public virtual async Task SaveChanges()
    {
        var result = await Context.SaveChangesAsync();
        Console.WriteLine(result);
    }

    public virtual bool DeleteInventory(Inventory inventory)
    {
        return false;
    }
}