using System.Linq.Expressions;
using BookStoreEF.Data;
using BookStoreEF.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEF.Management;

public class InventoryRepository : GenericRepository<Inventory>
{
    public InventoryRepository(BookStoreContext context) : base(context)
    {
    }
    
    public override Inventory Update(Inventory entity)
    {
        var inventory = Context.Inventories.FirstOrDefault(
            i =>
                i.StoreId == entity.StoreId &&
                i.Isbn == entity.Isbn
        );

        inventory.Quantity = entity.Quantity;

        if (inventory.Quantity < 0)
        {
            inventory.Quantity = 0;
        }
        
        return base.Update(inventory);
    }

    public override async Task<IEnumerable<Inventory>> All()
    {
        return await Context.Inventories
            .AsNoTracking()
            .Include(i => i.IsbnNavigation)
            .Include(i => i.Store)
            .ToListAsync();
    }

    public override async Task<IEnumerable<Inventory>> Find(Expression<Func<Inventory, bool>> predicate)
    {
        return await Context.Inventories
            .Include(i => i.IsbnNavigation)
            .Include(i => i.Store)
            .Where(predicate)
            .ToListAsync();
    }

    public override async Task<Inventory?> FindOne(Expression<Func<Inventory, bool>> predicate)
    {
        return await Context.Inventories
            .Include(i => i.IsbnNavigation)
            .Include(i => i.Store)
            .FirstOrDefaultAsync(predicate);
    }

    public override bool DeleteInventory(Inventory inventory)
    {
        Context.Inventories.Remove(inventory);
        return true;
    }
}