using BookStoreEF.Data;
using BookStoreEF.Models;

namespace BookStoreEF.Management;

public class AuthorRepository : GenericRepository<Author>
{
    private static IRepository<Author> authorRepository;
    
    public AuthorRepository(BookStoreContext context) : base(context)
    {
    }

    public override Author Update(Author entity)
    {
        var author = Context.Authors.FirstOrDefault(a => a.AuthorId == entity.AuthorId);

        if (!string.IsNullOrEmpty(entity.FirstName)) author.FirstName = entity.FirstName;
        if (!string.IsNullOrEmpty(entity.LastName)) author.LastName = entity.LastName;

        return base.Update(author);
    }
}