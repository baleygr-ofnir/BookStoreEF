using System.Linq.Expressions;
using BookStoreEF.Data;
using BookStoreEF.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEF.Management;

public class StoreRepository : GenericRepository<Store>
{
    public StoreRepository(BookStoreContext context) : base(context)
    {
    }

    public override Store Update(Store entity)
    {
        var store = Context.Stores
            .FirstOrDefault(s => s.StoreId == entity.StoreId);

        store.StoreName = entity.StoreName;
        store.StreetAddress = entity.StreetAddress;
        store.City = entity.City;
        store.Region = entity.Region;
        store.PostalCode = entity.PostalCode;
        store.Country = entity.Country;
        store.PhoneNumber = entity.PhoneNumber;
        store.EmailAddress = entity.EmailAddress;
        store.StoreManager = entity.StoreManager;

        return base.Update(store);
    }

    public override async Task<IEnumerable<Store>> Find(Expression<Func<Store, bool>> predicate)
    {
        return await Context.Stores
            .Where(predicate)
            .ToListAsync();
    }

    public override async Task<Store?> FindOne(Expression<Func<Store, bool>> predicate)
    {
        return await Context.Stores
            .Include(s => s.Inventories)
            .FirstOrDefaultAsync(predicate);
    }
}