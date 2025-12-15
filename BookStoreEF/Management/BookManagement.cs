using BookStoreEF.Data;
using BookStoreEF.Management;
using BookStoreEF.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEF;

public static class BookManagement
{
    private static IRepository<Book> bookRepository;
    
    private static readonly List<string> _options = new()
    {
        "Add new book",
        "List all books",
        "Find book by ISBN",
        "Find books by author",
        "Update an existing book",
        "Delete a book",
        "Exit"
    };
    
    private static async Task<Book> Setup(bool updating = false)
    {
        Book book;

        Console.Clear();
        if (updating)
        {
            Console.WriteLine("Enter nothing if not updating...");
            book = await Selection();
        }
        else
        {
            book = new Book();

            var author = await AuthorManagement.Selection(true);

            var publisher = await PublisherManagement.Selection(true);
            
            Console.Write("Enter book ISBN13 (only digits): ");
            book.Isbn = BookStoreManager.ValidInput(13, $"{BookStoreManager.ValidInput()}"); 
            
            Console.Write("Enter book publication date (e.g. 2025-02-04): ");
            book.PublicationDate = DateOnly.TryParse(Console.ReadLine(), out DateOnly publicationDate) ? publicationDate : DateOnly.Parse("2025-02-04");

            book.AuthorId = author.AuthorId;
            book.PublisherId = publisher.PublisherId;
        }

        Console.Write("Enter book title: ");
        book.Title = Console.ReadLine();

        Console.Write("Enter book price (e.g. 12.30): ");
        book.Price = decimal.TryParse(Console.ReadLine(), out decimal price) ? price : 0;

        Console.Write("Enter book language (e.g. \"en\", \"sv\"): ");
        book.Language = BookStoreManager.ValidInput(2, Console.ReadLine().ToLower());

        return book;
    }

    public static async Task Open(BookStoreContext context)
    {
        bookRepository = new BookRepository(context);
        string isbn;
        IEnumerable<Book> books;
        Author author;
        int choice = BookStoreManager.SelectionMenu("Books", _options);
        Console.Clear();
        
        switch (choice)
        {
            // Create Book
            case 0:
                var newBook = await Setup();
                await bookRepository.Add(newBook);
                await bookRepository.SaveChanges();
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
                author = await AuthorManagement.Selection(true);
                books = await bookRepository.Find(b => b.Author == author);
                (books as List<Book>).ForEach(Console.WriteLine);
                break;
            // Update book
            case 4:
                var updatedBook = await Setup(true);
                
                var updateResult = bookRepository.Update(updatedBook);
                if (updateResult == null)
                {
                    Console.WriteLine($"Error updating book, validate entered data: {updatedBook}");
                }
                else
                {
                    await bookRepository.SaveChanges();
                    Console.WriteLine($"Successfully updated book {updatedBook}");
                }
                break;
            // Delete book
            case 5:
                var bookToDelete = await Selection();
                var deleteResult = await bookRepository.Delete(bookToDelete.Isbn);
                if (deleteResult)
                {
                    Console.WriteLine($"Deletion of \"{bookToDelete}\" was successful.");
                    await bookRepository.SaveChanges();
                }
                else
                {
                    Console.WriteLine($"Book {bookToDelete} deletion was unsuccessful.");
                }
                break;
        }
    }

    public static async Task<Book> Selection(bool fromOther = false)
    {
        if (fromOther) bookRepository = new BookRepository(new BookStoreContext());
        
        var books = await bookRepository.All() as List<Book>;
        int bookIndex = BookStoreManager.SelectionMenu("Select Book",
            books.Select(b => $"{b}").ToList());
        var book = books[bookIndex];

        return book;
    }
}