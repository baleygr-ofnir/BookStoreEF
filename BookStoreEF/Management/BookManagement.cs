using BookStoreEF.Data;
using BookStoreEF.Management;
using BookStoreEF.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEF;

public static class BookManagement
{
    private static IRepository<Author> authorRepository;
    private static IRepository<Book> bookRepository;
    private static IRepository<Publisher> publisherRepository;
    
    public static List<string> Options = new()
    {
        "Add new book",
        "List all books",
        "Find book by ISBN",
        "Find books by author",
        "Update an existing book",
        "Delete a book"
    };

    public static async Task Open(BookStoreContext context)
    {
        authorRepository = new AuthorRepository(context);
        bookRepository = new BookRepository(context);
        publisherRepository = new PublisherRepository(context);
        string isbn;
        string authorFirstName;
        string authorLastName;
        string publisherName;
        IEnumerable<Book> books;
        Author author;
        Publisher publisher;
        int choice = BookStoreManager.SelectionMenu("Manage Books", BookManagement.Options);
        Console.Clear();
        
        switch (choice)
        {
            // Create Book
            case 0:
                var newBook = await SetupNew();
                

                
                var result = await bookRepository.Add(newBook);
                if (result == null)
                {
                    Console.WriteLine($"Error creating book, validate entered data: {newBook}");
                }
                else
                {
                    Console.WriteLine($"Book successfully created: {result}");
                }
                break;
            // Get All Books
            case 1: 
                books = await bookRepository.All();
                (books as List<Book>).ForEach(Console.WriteLine);
                break;
            // Get Book by ISBN
            case 2:
                Console.WriteLine("Enter book ISBN: (13 digits)");
                isbn = BookStoreManager.ValidInput(13, $"{BookStoreManager.ValidInput()}");
                var foundBook = await bookRepository.FindOne(b => b.Isbn == isbn);
                Console.WriteLine(foundBook);
                break;
            // Get books by author name
            case 3:
                Console.Write("Enter author first name: ");
                authorFirstName = Console.ReadLine();
                Console.Write("Enter author last name: ");
                authorLastName = Console.ReadLine();
                books = await bookRepository.Find(
                    b =>
                        b.Author.FirstName.ToLower().Equals(authorFirstName.ToLower()) &&
                        b.Author.LastName.ToLower().Equals(authorLastName.ToLower())
                );
                (books as List<Book>).ForEach(Console.WriteLine);
                break;
            // Update book
            case 4:
                isbn = BookStoreManager.ValidInput(13, $"{BookStoreManager.ValidInput()}");
                var existingBook = await bookRepository.Get(isbn);
                if (existingBook == null)
                {
                    Console.WriteLine("No book was found with supplied ISBN, returning...");
                    break;
                }
                var updatedBook = await SetupNew(true);
                updatedBook.Isbn = isbn;
                
                var updateResult = bookRepository.Update(updatedBook);
                if (updateResult == null)
                {
                    Console.WriteLine($"Error updating book, validate entered data: {updatedBook}");
                }
                else
                {
                    Console.Write($"Successfully updated book {updateResult}");
                }
                break;
            // Delete book
            case 5:
                Console.Write("Enter ISBN of book to delete: ");
                string isbnToDelete = BookStoreManager.ValidInput(13, $"{BookStoreManager.ValidInput()}");
                var deleteResult = await bookRepository.Delete(isbnToDelete);
                if (deleteResult)
                {
                    Console.WriteLine($"Book with ISBN {isbnToDelete} deletion was successful.");
                    await bookRepository.SaveChanges();
                }
                else
                {
                    Console.WriteLine($"Book with ISBN {isbnToDelete} deletion was unsuccessful.");
                }
                break;
        }
    }

    public static async Task<Book> SetupNew(bool updating = false)
    {
        var book = new Book();
        var authors = await authorRepository.All() as List<Author>;
        int authorIndex = BookStoreManager.SelectionMenu("Select Author",
            authors.Select(a => $"{a.FirstName} {a.LastName}").ToList());
        var author = authors[authorIndex];
        book.AuthorId = author.AuthorId;
        Console.WriteLine(author);
        if (!updating)
        {
            Console.Write("Enter book ISBN: ");
            book.Isbn = Console.ReadLine();
        }

        Console.Write("Enter book title: ");
        book.Title = Console.ReadLine();

        Console.Write("Enter book price (e.g. 12.30): ");
        book.Price = decimal.TryParse(Console.ReadLine(), out decimal price) ? price : 0;

        Console.Write("Enter book language (e.g. \"en\", \"sv\"): ");
        book.Language = Console.ReadLine();

        Console.Write("Enter book publication date (e.g. 2025-02-04): ");
        book.PublicationDate = DateOnly.Parse(Console.ReadLine());

        var publishers = await publisherRepository.All() as List<Publisher>;
        Publisher publisher;
        List<string> publisherNames = publishers.Select(p => p.PublisherName).ToList();
        publisherNames.Add("Publisher name not available");
        int publisherChoice = BookStoreManager.SelectionMenu("Select Publisher",publisherNames);
        if (publisherNames[publisherChoice] == publisherNames.Last())
        {
            Console.Write("Enter name of new publisher: ");
            string publisherName = Console.ReadLine();
            
            Console.Write("Enter publisher country: ");
            string publisherCountry = Console.ReadLine();
            publisher = new Publisher()
            {
                PublisherName = publisherName,
                Country = publisherCountry
            };
        }
        else
        {
            publisher = publishers[publisherChoice];
        }
        book.PublisherId = publisher.PublisherId;

        return book;
    }
}