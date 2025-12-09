using BookStoreEF.Data;
using BookStoreEF.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEF.Management;

public static class BookStoreManager
{
    private static List<string> menuOptions = new List<string>()
    {
        "Books",
        "Authors",
        "Customers",
        "Exit"
    };
    private static BookStoreContext _context = new BookStoreContext();

    public static async Task Open()
    {
        bool running = true;
        while (running)
        {
            int choice = SelectionMenu("Book Store Manager", menuOptions);
            switch (choice)
            {
                case 0:
                    await Books();
                    break;
                case 1:
                    await Authors();
                    break;
                case 2:
                    await Customers();
                    break;
                case 3:
                    running = false;
                    break;
            }
        }
    }
    
    static async Task Books()
    {
        var service = new BookDbService(_context);
        string isbn;
        string authorFirstName;
        string authorLastName;
        string publisherName;
        List<Book> books;
        Author author;
        Publisher publisher;
        int choice = SelectionMenu("Books", BookManagement.Options);
        Console.Clear();
        
        switch (choice)
        {
            case 0:
                var newBook = BookManagement.SetupBook();
                
                Console.Write("Enter author first name: ");
                authorFirstName = Console.ReadLine();
                Console.Write("Enter author last name: ");
                authorLastName = Console.ReadLine();
                author = await _context.Authors.FirstOrDefaultAsync(author => 
                        author.FirstName.ToLower().Equals(authorFirstName) && author.LastName.ToLower().Equals(authorLastName)
                );
                if (author == null)
                {
                    author = new Author()
                    {
                        FirstName = authorFirstName,
                        LastName = authorLastName
                    };
                    newBook.Author = author;
                }
                else
                {
                    newBook.AuthorId = author.AuthorId;
                    newBook.Author = author;
                }
                
                Console.Write("Enter publisher name");
                publisherName = Console.ReadLine();
                publisher =
                    await _context.Publishers.FirstOrDefaultAsync(publisher =>
                        publisherName.ToLower().Equals(publisher.PublisherName));
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
                
                var result = await service.CreateBook(newBook);
                if (result == null)
                {
                    Console.WriteLine($"Error creating book, validate entered data: {newBook}");
                }
                else
                {
                    Console.WriteLine($"Book successfully created: {result}");
                }
                break;
            case 1:
                books = await service.GetBooks();
                foreach (var book in books)
                {
                    Console.WriteLine(book);
                }
                break;
            case 2:
                Console.WriteLine("Enter book ISBN: (13 digits)");
                isbn = ValidInput(13, $"{ValidInput()}");
                var foundBook = await service.GetBook(isbn);
                Console.WriteLine(foundBook);
                break;
            case 3:
                Console.Write("Enter author first name: ");
                authorFirstName = Console.ReadLine();
                Console.Write("Enter author last name: ");
                authorLastName = Console.ReadLine();
                books = await service.GetBooksByAuthor(authorFirstName, authorLastName);
                foreach (var book in books) Console.WriteLine(book);
                break;
            case 4:
                isbn = ValidInput(13, $"{ValidInput()}");
                var existingBook = await service.GetBook(isbn);
                var updatedBook = BookManagement.SetupBook(true);

                
                updatedBook.Isbn = isbn;
                updatedBook.AuthorId = existingBook.AuthorId;
                updatedBook.PublisherId = updatedBook.PublisherId;
                existingBook = null;
                var updateResult = await service.UpdateBook(updatedBook);
                if (updateResult == null)
                {
                    Console.WriteLine($"Error creating book, validate entered data: {updatedBook}");
                }
                else
                {
                    Console.Write($"Successfully updated book {updateResult}");
                }
                break;
            case 5:
                Console.Write("Enter ISBN of book to delete: ");
                string isbnToDelete = ValidInput(13, $"{ValidInput()}");
                var deleteResult = service.DeleteBook(isbnToDelete);
                break;
        }
        Console.WriteLine("Enter any key to continue...");
        Console.ReadKey(true);
    }
    
    static async Task Authors()
    {
        throw new NotImplementedException();
    }

    static async Task Customers()
    {
        throw new NotImplementedException();

    }

    static int SelectionMenu(string title, List<string> menuOptions)
    {
        ConsoleKeyInfo keyPressed;
        int currentIndex = 0;
        do
        {
            Console.Clear();
            Console.WriteLine($"Manage {title} [Press Enter to Select]");
            for (int i = 0; i < menuOptions.Count; i++)
            {
                if (i == currentIndex)
                {
                    Console.Write(">");
                }
                else
                {
                    Console.Write(" ");
                }
                Console.WriteLine(menuOptions[i]);
            }

            keyPressed = Console.ReadKey(true);

            switch (keyPressed.Key)
            {
                case ConsoleKey.UpArrow:
                    currentIndex = (currentIndex == 0) ? menuOptions.Count - 1 : currentIndex - 1;
                    break;
                case ConsoleKey.DownArrow:
                    currentIndex = (currentIndex == menuOptions.Count - 1) ? 0 : currentIndex + 1;
                    break;
            }
        } while (keyPressed.Key != ConsoleKey.Enter);

        
        return currentIndex;
    }
    
    static string ValidInput(int requiredCharCount, string inputString)
    {
        do
        {
            if (inputString.Length != requiredCharCount)
            {
                Console.Write($"Incorrect amount of characters, try again: ");
                if (requiredCharCount == 13) // ISBN13 input check
                {
                    inputString = $"{ValidInput()}";
                }
                else
                {
                    inputString = Console.ReadLine();
                }
            }
        } while (inputString.Length != requiredCharCount);

        return inputString;
    }

    static long ValidInput()
    {
        bool validInt;
        long integer;
        do
        {
            Console.Write("Enter integer: ");
            validInt = long.TryParse(Console.ReadLine(), out integer);
            if (!validInt) Console.WriteLine("Enter a valid integer.");
        } while (!validInt);
        

        return integer;
    }
}