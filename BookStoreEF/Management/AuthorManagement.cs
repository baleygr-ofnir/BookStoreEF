using BookStoreEF.Data;
using BookStoreEF.Models;

namespace BookStoreEF.Management;

public static class AuthorManagement
{
    private static IRepository<Author> authorRepository; 
    private static List<string> _options = new()
    {
        "Add new author",
        "List all authors",
        "Update existing author",
        "Delete existing author",
        "Exit"
    };

    private static async Task<Author> Setup(bool updating = false)
    {
        Console.Clear();
        Author author;
        if (updating)
        {
            Console.WriteLine("Enter nothing if not updating...");
            author = await Selection();
        }
        else
        {
            author = new Author();
        }
        
        Console.Write("Enter author's first name: ");
        string firstName = Console.ReadLine();

        Console.Write("Enter author's last name: ");
        string lastName = Console.ReadLine();

        Console.Write("Enter author's birthdate (e.g. 1952-05-12): ");
        DateOnly birthdate = DateOnly.Parse(Console.ReadLine());

        author.FirstName = firstName;
        author.LastName = lastName;
        author.Birthdate = birthdate;

        return author;
    }
    
    public static async Task Open(BookStoreContext context)
    {
        authorRepository = new AuthorRepository(context);
        int menuChoice = BookStoreManager.SelectionMenu("Author Management", _options);

        Console.Clear();
        switch (menuChoice)
        {
            case 0:
                var newAuthor = await Setup();
                await authorRepository.Add(newAuthor);
                await authorRepository.SaveChanges();
                Console.WriteLine($"Author saved: \n{newAuthor}");
                break;
            case 1:
                var authors = await authorRepository.All() as List<Author>;
                Console.WriteLine("Available authors:");
                authors.ForEach(Console.WriteLine);
                break;
            case 2:
                // Update
                var updatedAuthor = await Setup(true);
                authorRepository.Update(updatedAuthor);
                await authorRepository.SaveChanges();
                break;
            case 3:
                // Delete
                var authorToDelete = await Selection();
                await authorRepository.Delete(authorToDelete.AuthorId);
                await authorRepository.SaveChanges();
                break;
        }
    }

    public static async Task<Author> Selection(bool fromOther = false)
    {
        if (fromOther) authorRepository = new AuthorRepository(new BookStoreContext());
        
        var authors = await authorRepository.All() as List<Author>;
        int authorIndex = BookStoreManager.SelectionMenu("Select Author",
            authors.Select(a => $"{a.FirstName} {a.LastName}").ToList());
        var author = authors[authorIndex];
        
        return author;
    }
}