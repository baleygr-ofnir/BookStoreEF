using BookStoreEF.Data;
using BookStoreEF.Models;

namespace BookStoreEF.Management;

public static class AuthorManagement
{
    private static IRepository<Author> authorRepository; 
    private static List<string> menuOptions = new()
    {
        "Add new author",
        "List all authors",
        "Update existing author",
        "Delete existing author"
    };
    
    public static async Task Open(BookStoreContext context)
    {
        authorRepository = new AuthorRepository(context);
        int menuChoice = BookStoreManager.SelectionMenu("Author Management", menuOptions);

        switch (menuChoice)
        {
            case 0:
                var newAuthor = SetupAuthor();
                await authorRepository.Add(newAuthor);
                await authorRepository.SaveChanges();
                break;
            case 1:
                var authors = await authorRepository.All() as List<Author>;
                authors.ForEach(Console.WriteLine);
                break;
            case 2:
                // Update
                break;
            case 3:
                // Delete
                break;
        }
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