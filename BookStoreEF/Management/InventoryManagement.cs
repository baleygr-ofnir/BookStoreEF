using BookStoreEF.Data;
using BookStoreEF.Models;

namespace BookStoreEF.Management;

public static class InventoryManagement
{
    private static IRepository<Book> bookRepository;
    private static IRepository<Inventory> inventoryRepository;
    private static IRepository<Store> storeRepository;

    private static List<string> menuOptions = new()
    {
        "Adjust inventory",
        "List inventory of a selected store",
        "Delete inventory entry"
    };
    public static async Task Open(BookStoreContext context)
    {
        bookRepository = new BookRepository(context);
        inventoryRepository = new InventoryRepository(context);
        storeRepository = new StoreRepository(context);

        int choice = BookStoreManager.SelectionMenu("Manage Inventory", menuOptions);

        switch (choice)
        {
            case 0:
                var newInventory = await Setup();
                var existingInventory = await inventoryRepository.FindOne(i => (i.StoreId == newInventory.StoreId && i.Isbn == newInventory.Isbn)); 
                var doesExist = existingInventory == null
                    ? await inventoryRepository.Add(newInventory)
                    : inventoryRepository.Update(newInventory);
                await inventoryRepository.SaveChanges();
                Console.WriteLine($"Inventory for selected book at selected store was adjusted, new quantity: {existingInventory.Quantity}");
                break;
            case 1:
                var stores = await storeRepository.All();
                List<string> storeNames = stores.Select(s => s.StoreName).ToList();
                int storeChoice = BookStoreManager.SelectionMenu("Choose which store inventory to list", storeNames);
                var store = stores.FirstOrDefault(s => s.StoreName.Equals(storeNames[storeChoice]));
                var inventories = await inventoryRepository.Find(i => i.StoreId == store.StoreId) as List<Inventory>;
                int totalBooks = 0;
                inventories.ForEach(i => totalBooks += i.Quantity);
                Console.WriteLine($"{store.StoreName}:");
                inventories.ForEach(Console.WriteLine);
                Console.WriteLine($"Total amount of books in store: {totalBooks}");                
                break;
            case 2:
                // Delete
                break;
        }
    }

    public static async Task<Inventory> Setup()
    {
        var inventory = new Inventory();
        
        var books = await bookRepository.All() as List<Book>;
        List<string> bookTitles = books.Select(b => b.Title).ToList();
        int bookSelection = BookStoreManager.SelectionMenu("Select which book to manage inventory for", bookTitles);
        var book = books.FirstOrDefault(b => b.Title == bookTitles[bookSelection]);
        inventory.Isbn = book.Isbn;
        
        var stores = await storeRepository.All() as List<Store>;
        List<string> storeNames = stores.Select(s => s.StoreName).ToList();
        var storeSelection = BookStoreManager.SelectionMenu("Select which store to adjust inventory (enter a negative value to remove)", storeNames);
        var store = stores.FirstOrDefault(s => s.StoreName == storeNames[storeSelection]);
        inventory.StoreId = store.StoreId;
        
        Console.Write("Enter quantity to add: ");
        inventory.Quantity = (int) BookStoreManager.ValidInput();
        
        return inventory;
    }
}