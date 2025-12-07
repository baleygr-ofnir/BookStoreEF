using BookStoreEF.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEF.Data;

public class DbService
{
    private BookStoreContext _context;

    public DbService(BookStoreContext context)
    {
        _context = context;
    }
    
    // Author Actions

    public async Task<Author> CreateAuthor(Author author)
    {
        try
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException(
                "Failed to create author. Check for duplicate Author ID or invalid references.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unknown error occurred.", ex);
        }
    }

    public async Task<List<Author>> GetAuthors()
    {
        return await _context.Authors
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Author> GetAuthor()
    {
        
    }
    
    // Books Actions
    
    public async Task<Book> CreateBook(Book book)
    {
        try
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException(
                "Failed to create book. Check for duplicate ISBN or invalid references.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unknown error occurred.", ex);
        }
    }

    public async Task<List<Book>> GetBooks()
    {
        return await _context.Books
            .AsNoTracking()
            .Include(book => book.Author)
            .Include(book => book.Publisher)
            .ToListAsync();
    }

    public async Task<List<Book>> GetBooksByAuthor(int authorId)
    {
        return await _context.Books
            .AsNoTracking()
            .Where(book => book.AuthorId == authorId)
            .Include(book => book.Publisher)
            .ToListAsync();
    }

    public async Task<Book> GetBook(string isbn)
    {
        return await _context.Books.FindAsync(isbn);
    }

    public async Task<Inventory> GetBookInventory(string isbn)
    {
        return await _context.Inventories.FindAsync(isbn);
    }
    
    public async Task<Book> UpdateBook(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<bool> DeleteBook(string isbn)
    {
        var book = await _context.Books.FindAsync(isbn);
        if (book == null)
        {
            return false;
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }
    
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
    
    // Customer Actions

    public async Task<Customer> CreateCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<List<Customer>> GetCustomers()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer> GetCustomer(int id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task<List<Order>> GetCustomerOrders(int customerId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(order => order.CustomerId == customerId)
            .Include(order => order.OrderDetails)
            .ThenInclude(orderDetails => orderDetails.IsbnNavigation)
            .Include(order => order.Store)
            .OrderByDescending(order => order.OrderDate)
            .ToListAsync();
    }
    
    public async Task<Customer> UpdateCustomer(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<bool> DeleteCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return false;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return true;
    }
}