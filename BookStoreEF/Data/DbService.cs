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
    

    

}