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

        if (!string.IsNullOrEmpty(entity.StoreName)) store.StoreName = entity.StoreName;
        if (!string.IsNullOrEmpty(entity.StreetAddress)) store.StreetAddress = entity.StreetAddress;
        if (!string.IsNullOrEmpty(entity.City)) store.City = entity.City;
        if (!string.IsNullOrEmpty(entity.Region)) store.Region = entity.Region;
        if (!string.IsNullOrEmpty(entity.PostalCode)) store.PostalCode = entity.PostalCode;
        if (!string.IsNullOrEmpty(entity.Country)) store.Country = entity.Country;
        if (!string.IsNullOrEmpty(entity.PhoneNumber)) store.PhoneNumber = entity.PhoneNumber;
        if (!string.IsNullOrEmpty(entity.EmailAddress)) store.EmailAddress = entity.EmailAddress;
        if (!string.IsNullOrEmpty(entity.StoreManager)) store.StoreManager = entity.StoreManager;

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