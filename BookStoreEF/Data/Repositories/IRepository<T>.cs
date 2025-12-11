using System.Linq.Expressions;
using BookStoreEF.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookStoreEF.Management;

public interface IRepository<T> where T : class
{
    Task<T> Add(T entity);
    Task<T> Update(T entity);
    Task<T> Get(int id);
    Task<T> Get(string id);
    Task<IEnumerable<T>> All();
    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
    Task<T?> FindOne(Expression<Func<T, bool>> predicate);
    Task<bool> Delete(int id);
    Task<bool> Delete(string id);
    Task SaveChanges();
}