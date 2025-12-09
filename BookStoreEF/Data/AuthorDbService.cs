using BookStoreEF.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEF.Data;

public class AuthorDbService
{
    private BookStoreContext _context;
    public AuthorDbService(BookStoreContext context)
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

    public async Task<Author> GetAuthor(int id)
    {
        return await _context.Authors.FirstOrDefaultAsync(author => author.AuthorId == id);
    }

    public async Task<Author> UpdateAuthor(int id, Author updatedAuthor)
    {
        var existingAuthor = await GetAuthor(id);
        if (existingAuthor == null) return null;
        
        updatedAuthor.AuthorId = id;
        _context.Authors.Update(updatedAuthor);
        await _context.SaveChangesAsync();
        return updatedAuthor;
    }

    public async Task<bool> DeleteAuthor(int id)
    {
        var author = await GetAuthor(id);
        if (author == null) return false;

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
        return true;
    }
}