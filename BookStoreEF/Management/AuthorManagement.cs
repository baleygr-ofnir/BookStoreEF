using BookStoreEF.Data;
using BookStoreEF.Models;

namespace BookStoreEF.Management;

public static class AuthorManagement
{
    private static IRepository<Author> authorRepository; 
    private static IRepository<Book> bookRepository; 
    private static IRepository<Publisher> publisherRepository; 
    private static List<string> menuOptions = new()
    {
        "Add new author",
        "List all authors",
        "Find author",
        ""
    };
    
    public static async Task Open(BookStoreContext context)
    {
        
    }

    public static Author SetupAuthor(bool updating = false)
    {
        Console.Write("Enter author's first name: ");
        string firstName = Console.ReadLine();

        Console.Write("Enter author's last name: ");
        string lastName = Console.ReadLine();

        Console.Write("Enter author's birthdate (e.g. 1952-05-12): ");
        DateOnly birthdate = DateOnly.Parse(Console.ReadLine());

        return new Author()
        {
            FirstName = firstName,
            LastName = lastName,
            Birthdate = birthdate
        };
    }
}