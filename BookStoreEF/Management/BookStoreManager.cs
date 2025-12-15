using BookStoreEF.Data;
using BookStoreEF.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEF.Management;

public static class BookStoreManager
{
    private static List<string> _options = new ()
    {
        "Books",
        "Authors",
        "Publishers",
        "Inventory",
        "Store",
        "Exit"
    };
    private static BookStoreContext _context = new ();

    public static async Task Open()
    {
        bool running = true;
        while (running)
        {
            int choice = SelectionMenu("Book Store Manager", _options);
            switch (choice)
            {
                case 0:
                    await BookManagement.Open(_context);
                    break;
                case 1:
                    await AuthorManagement.Open(_context);
                    break;
                case 2:
                    await PublisherManagement.Open(_context);
                    break;
                case 3:
                    await InventoryManagement.Open(_context);
                    break;
                case 4:
                    await StoreManagement.Open(_context);
                    break;
                case 5:
                    running = false;
                    break;
            }

            if (choice < 5)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
            }
        }
    }
    
    public static int SelectionMenu(string title, List<string> menuOptions)
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
    
    public static string ValidInput(int requiredCharCount, string inputString)
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
                    inputString = ValidInput(requiredCharCount, Console.ReadLine());
                }
            }
        } while (inputString.Length != requiredCharCount);

        return inputString;
    }

    public static long ValidInput()
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