using BookStoreEF.Models;

namespace BookStoreEF;

public static class BookManagement
{
    public static List<string> Options = new List<string>()
    {
        "Add new book",
        "List all books",
        "Find book by ISBN",
        "Find books by author",
        "Update an existing book",
        "Delete a book"
    };

    public static Book SetupBook(bool updating = false)
    {
        var book = new Book();

        if (!updating)
        {
            Console.Write("Enter book ISBN: ");
            book.Isbn = Console.ReadLine();
        }
        
        Console.Write("Enter book title: ");
        book.Title = Console.ReadLine();
        
        Console.Write("Enter book price: (e.g. 12.30)");
        book.Price = decimal.TryParse(Console.ReadLine(), out decimal price) ? price : 0;
        
        Console.Write("Enter book language: (e.g. \"en\", \"sv\")");
        book.Language = Console.ReadLine();
        
        Console.Write("Enter book publication date: (e.g. 20250204");
        book.PublicationDate = DateOnly.Parse(Console.ReadLine());

        return book;
    }
}