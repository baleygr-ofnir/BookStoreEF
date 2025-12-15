using BookStoreEF.Data;
using BookStoreEF.Models;

namespace BookStoreEF.Management;

public static class InventoryManagement
{
    private static IRepository<Book> bookRepository;
    private static IRepository<Inventory> inventoryRepository;
    private static IRepository<Store> storeRepository;
    private static readonly List<string> _options = new()
    {
        "Add new inventory",
        "List inventory of a selected store",
        "Update inventory",
        "Delete inventory entry",
        "Exit"
    };

    private static async Task<Inventory> Setup(bool updating = false)
    {
        var inventory = new Inventory();
        
        Console.Clear();
        if (!updating)
        {
            var book = await BookManagement.Selection(true);
            inventory.Isbn = book.Isbn;
            
            var store = await StoreManagement.Selection(true);
            inventory.StoreId = store.StoreId;
        }
        else
        {
            inventory = await Selection();
        }
        Console.Write("Enter quantity: ");
        inventory.Quantity = (int) BookStoreManager.ValidInput();
        
        return inventory;
    }

    public static async Task Open(BookStoreContext context)
    {
        inventoryRepository = new InventoryRepository(context);
        int choice = BookStoreManager.SelectionMenu("Manage Inventory", _options);

        Console.Clear();
        
        switch (choice)
        {
            case 0:
                var newInventory = await Setup();
                var addedInventory = await inventoryRepository.Add(newInventory);
                await inventoryRepository.SaveChanges();
                Console.WriteLine($"Inventory was added at the selected store: {addedInventory}");
                break;
            case 1:
                var store = await StoreManagement.Selection(true);
                var inventories = await inventoryRepository.Find(i => i.StoreId == store.StoreId) as List<Inventory>;
                int totalBooks = 0;
                inventories.ForEach(i => totalBooks += i.Quantity);
                Console.WriteLine($"{store.StoreName}:");
                inventories.ForEach(Console.WriteLine);
                Console.WriteLine($"Total amount of books in store: {totalBooks}");                
                break;
            case 2:
                //Update
                var updatedInventory = await Setup(true);
                inventoryRepository.Update(updatedInventory);
                await inventoryRepository.SaveChanges();
                Console.WriteLine($"Updated:\n {updatedInventory}");
                break;
            case 3:
                // Delete
                var inventoryToDelete = await Selection();
                var result = inventoryRepository.DeleteInventory(inventoryToDelete);
                await inventoryRepository.SaveChanges();
                Console.WriteLine(result ? "Deleted" : "Error deleting");
                break;
        }
    }


    public static async Task<Inventory> Selection(bool fromOther = false)
    {
        if (fromOther) inventoryRepository = new InventoryRepository(new BookStoreContext());
        var store = await StoreManagement.Selection(true);
        var inventories = await inventoryRepository.All() as List<Inventory>;
        int inventoryIndex = BookStoreManager.SelectionMenu("Select Inventory",
            inventories.Where(i => i.StoreId == store.StoreId).Select(i => $"{i}").ToList());
        var inventory = inventories[inventoryIndex];
        
        return inventory;
        
    }
}