using BookStoreEF.Data;
using BookStoreEF.Models;

namespace BookStoreEF.Management;

public static class StoreManagement
{
    private static IRepository<Store> storeRepository;
    private static readonly List<string> _options = new()
    {
        "Add new store",
        "List available stores",
        "Update existing store",
        "Delete existing store",
        "Exit"
    };
    
    private static async Task<Store> Setup(bool updating = false)
    {
        Store store;

        Console.Clear();
        if (updating)
        {
            Console.WriteLine("Enter nothing if not updating...");
            store = await Selection();
        }
        else
        {
            store = new Store();
        }

        Console.Write("Enter store name: ");
        store.StoreName = Console.ReadLine();
        
        Console.Write("Enter street address: ");
        store.StreetAddress = Console.ReadLine();
        
        Console.Write("Enter city: ");
        store.City = Console.ReadLine();
        
        Console.Write("Enter region: ");
        store.Region = Console.ReadLine();
        
        Console.Write("Enter postal code: ");
        store.PostalCode = Console.ReadLine();
        
        Console.Write("Enter country: ");
        store.Country = Console.ReadLine();
        
        Console.Write("Enter phone number: ");
        store.PhoneNumber = Console.ReadLine();
        
        Console.Write("Enter e-mail address: ");
        store.EmailAddress = Console.ReadLine();
        
        Console.Write("Enter name of store manager: ");
        store.StoreManager = Console.ReadLine();
        
        return store;
    }
    
    public static async Task Open(BookStoreContext context)
    {
        storeRepository = new StoreRepository(context);
        int choice = BookStoreManager.SelectionMenu("Stores", _options);
    
        Console.Clear();
        
        switch (choice)
        {
            case 0:
                var newStore = await Setup();
                await storeRepository.Add(newStore);
                await storeRepository.SaveChanges();
                break;
            case 1:
                var stores = await storeRepository.All() as List<Store>;
                stores.ForEach(Console.WriteLine);
                break;
            case 2:
                var updatedStore = await Setup(true);
                storeRepository.Update(updatedStore);
                await storeRepository.SaveChanges();
                break;
            case 3:
                var storeToDelete = await Selection();
                await storeRepository.Delete(storeToDelete.StoreId);
                await storeRepository.SaveChanges();
                break;
        }
    }
    
    public static async Task<Store> Selection(bool fromOther = false)
    {
        if (fromOther) storeRepository = new StoreRepository(new BookStoreContext());
        
        var stores = await storeRepository.All() as List<Store>;
        int storeIndex = BookStoreManager.SelectionMenu("Select Store",
            stores.Select(a => a.StoreName).ToList());
        var store = stores[storeIndex];

        return store;
    }
}