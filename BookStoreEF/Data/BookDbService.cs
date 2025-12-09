using BookStoreEF.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEF.Data;

public class BookDbService
{
    private BookStoreContext _context;

    public BookDbService(BookStoreContext context)
    {
        _context = context;
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

    public async Task<List<Book>> GetBooksByAuthor(string firstName, string lastName)
    {
        return await _context.Books
            .AsNoTracking()
            .Where(book => book.Author.FirstName.Equals(firstName) && book.Author.LastName.Equals(lastName))
            .Include(book => book.Author)
            .Include(book => book.Publisher)
            .ToListAsync();
    }

    public async Task<Book?> GetBook(string isbn)
    {
        return await _context.Books
            .AsNoTracking()
            .Include(book => book.Author)
            .Include(book => book.Publisher)
            .FirstOrDefaultAsync(book => book.Isbn == isbn);
    }

    public async Task<Inventory> GetBookInventory(string isbn)
    {
        return await _context.Inventories.FindAsync(isbn);
    }
    
    public async Task<Book> UpdateBook(Book book)
    {
        _context.Books.Update(book);
        // Clear navigation properties if they're causing conflicts
        _context.Entry(book).Reference(b => b.Author).IsModified = false;
        _context.Entry(book).Reference(b => b.Publisher).IsModified = false;
        await _context.SaveChangesAsync();
        return await GetBook(book.Isbn);
    }

    public async Task<bool> DeleteBook(string isbn)
    {
        var book = await _context.Books.FindAsync(isbn);
        if (book == null) return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }
    
}