using BookStoreEF.Data;
using BookStoreEF.Models;

namespace BookStoreEF.Management;

public class AuthorRepository : GenericRepository<Author>
{
    private static IRepository<Author> authorRepository;
    
    public AuthorRepository(BookStoreContext context) : base(context)
    {
    }

    public override Task<Author> Update(Author entity)
    {
        var author = Context.Authors.FirstOrDefault(a => a.AuthorId == entity.AuthorId);

        author.FirstName = entity.FirstName;
        author.LastName = entity.LastName;
        author.Birthdate = entity.Birthdate;

        return base.Update(author);
    }
}