using System.Linq.Expressions;
using BookStoreEF.Data;
using BookStoreEF.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEF.Management;

public class BookRepository : GenericRepository<Book>
{
    public BookRepository(BookStoreContext context) : base(context)
    {
    }

    public override Book Update(Book entity)
    {
        var book = Context.Books
            .FirstOrDefault(b => b.Isbn == entity.Isbn);

        if (!string.IsNullOrEmpty(entity.Title)) book.Title = entity.Title;
        if (!string.IsNullOrEmpty(entity.Language)) book.Language = entity.Language;
        if (entity.Price > 0 && entity.Price != book.Price) book.Price = entity.Price;

        return base.Update(book);
    }

    public override async Task<IEnumerable<Book>> All()
    {
        return await Context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Publisher)
            .ToListAsync();
    }
    
    public override async Task<IEnumerable<Book>> Find(Expression<Func<Book, bool>> predicate)
    {
        return await Context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Publisher)
            .Where(predicate)
            .ToListAsync();
    }

    public override async Task<Book?> FindOne(Expression<Func<Book, bool>> predicate)
    {
        return await Context.Books
            .Include(b => b.Author)
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(predicate);
    }
}