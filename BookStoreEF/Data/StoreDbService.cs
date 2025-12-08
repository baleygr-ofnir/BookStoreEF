using BookStoreEF.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEF.Data;

public class StoreDbService(BookStoreContext context)
{
    private BookStoreContext _context = context;
    // Stores Actions

    public async Task<Store> CreateStore(Store store)
    {
        _context.Stores.Add(store);
        await _context.SaveChangesAsync();
        return store;
    }

    public async Task<List<Store>> GetStores()
    {
        return await _context.Stores.ToListAsync();
    }

    public async Task<Store> GetStore(int id)
    {
        return await _context.Stores.FindAsync(id);
    }

    public async Task<List<Inventory>> GetInventories()
    {
        return await _context.Inventories.ToListAsync();
    }

    public async Task<Inventory> GetInventory(int storeId, string isbn)
    {
        return await _context.Inventories.FindAsync(storeId, isbn);
    }

    public async Task<Inventory> GetStoreInventoryForBook(int storeId, string isbn)
    {
        return await _context.Inventories
            .FirstOrDefaultAsync(inventory => inventory.StoreId == storeId && inventory.Isbn == isbn);
    }
    
    public async Task<Store> UpdateStore(Store store)
    {
        _context.Stores.Update(store);
        await _context.SaveChangesAsync();
        return store;
    }

    public async Task<bool> DeleteStore(int id)
    {
        var store = await _context.Stores.FindAsync(id);
        if (store == null)
        {
            return false;
        }

        _context.Stores.Remove(store);
        await _context.SaveChangesAsync();
        return true;
    }

}