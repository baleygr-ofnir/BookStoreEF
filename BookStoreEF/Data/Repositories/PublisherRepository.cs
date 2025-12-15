using BookStoreEF.Data;
using BookStoreEF.Models;

namespace BookStoreEF.Management;

public class PublisherRepository : GenericRepository<Publisher>
{
    public PublisherRepository(BookStoreContext context) : base(context)
    {
    }
}