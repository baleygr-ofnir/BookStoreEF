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
                var newBook = SetupNew();
                
                Console.Write("Enter author first name: ");
                authorFirstName = Console.ReadLine();
                Console.Write("Enter author last name: ");
                authorLastName = Console.ReadLine();
                author = await authorRepository.FindOne(a =>
                    a.FirstName.ToLower().Equals(authorFirstName.ToLower()) &&
                    a.LastName.ToLower().Equals(authorLastName.ToLower())
                );
                if (author == null)
                {
                    author = new Author()
                    {
                        FirstName = authorFirstName,
                        LastName = authorLastName
                    };
                    await authorRepository.Add(author);
                    await authorRepository.SaveChanges();
                    newBook.AuthorId = author.AuthorId;
                }
                else
                {
                    newBook.AuthorId = author.AuthorId;
                    newBook.Author = author;
                }
                
                Console.Write("Enter publisher name");
                publisherName = Console.ReadLine();
                publisher = await publisherRepository.FindOne(
                    p =>
                        p.PublisherName.ToLower().Equals(publisherName.ToLower())
                );
                if (publisher == null)
                {
                    publisher = new Publisher() { PublisherName = publisherName };
                    newBook.Publisher = publisher;
                }
                else
                {
                    newBook.PublisherId = publisher.PublisherId;
                    newBook.Publisher = publisher;
                }
                
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
                foreach (var book in books)
                {
                    Console.WriteLine(book);
                }
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
                foreach (var book in books) Console.WriteLine(book);
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
                var updatedBook = SetupNew(true);
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
        Console.WriteLine("Enter any key to continue...");
        Console.ReadKey(true);
    }

    public static Book SetupNew(bool updating = false)
    {
        var book = new Book();

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

        return book;
    }
}