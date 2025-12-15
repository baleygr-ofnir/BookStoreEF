using BookStoreEF.Data;
using BookStoreEF.Models;

namespace BookStoreEF.Management;

public static class PublisherManagement
{
    private static IRepository<Publisher> publisherRepository;

    private static List<string> _options = new()
    {
        "Add new publisher",
        "List all publishers",
        "Update existing publisher",
        "Delete existing publisher",
        "Exit"
    };

    private static Publisher Setup(bool updating = false)
    {
        Console.Clear();
        if (updating) Console.WriteLine("Enter nothing for anything to leave unchanged...");
        
        Console.Write("Enter publisher name: ");
        string name = Console.ReadLine();
        
        Console.Write("Enter e-mail address: ");
        string email = Console.ReadLine();
        
        Console.Write("Enter link to website: ");
        string website = Console.ReadLine();
        
        Console.Write("Enter phone number");
        string phone = Console.ReadLine();
        
        Console.Write("Enter full name of contact person: ");
        string contact = Console.ReadLine();
        
        Console.Write("Enter street address: ");
        string address = Console.ReadLine();
        
        Console.Write("Enter city: ");
        string city = Console.ReadLine();
        
        Console.Write("Enter postal code: ");
        string postalCode = Console.ReadLine();
        
        Console.Write("Enter country: ");
        string country = Console.ReadLine();

        var publisher = new Publisher()
        {
            PublisherName = name,
            EmailAddress = email,
            Website = website,
            PhoneNumber = phone,
            ContactPerson = contact,
            StreetAddress = address,
            City = city,
            PostalCode = postalCode,
            Country = country
        };

        return publisher;
    }

    public static async Task Open(BookStoreContext context)
    {
        publisherRepository = new PublisherRepository(context);
        int menuChoice = BookStoreManager.SelectionMenu("Publisher", _options);
        
        Console.Clear();
        
        switch (menuChoice)
        {
            case 0:
                var newPublisher = Setup();
                await publisherRepository.Add(newPublisher);
                await publisherRepository.SaveChanges();
                break;
            case 1:
                Console.WriteLine("Stored publishers:");
                var publishers = await publisherRepository.All() as List<Publisher>;
                publishers.ForEach(Console.WriteLine);
                break;
            case 2:
                var existingPublisher = await Selection();
                Publisher updatedPublisher = Setup(true);
                updatedPublisher.PublisherId = existingPublisher.PublisherId;

                if (string.IsNullOrEmpty(updatedPublisher.PublisherName)) updatedPublisher.PublisherName = existingPublisher.PublisherName;
                if (string.IsNullOrEmpty(updatedPublisher.EmailAddress)) updatedPublisher.EmailAddress = existingPublisher.EmailAddress;
                if (string.IsNullOrEmpty(updatedPublisher.Website)) updatedPublisher.Website = existingPublisher.Website;
                if (string.IsNullOrEmpty(updatedPublisher.PhoneNumber)) updatedPublisher.PhoneNumber = existingPublisher.PhoneNumber;
                if (string.IsNullOrEmpty(updatedPublisher.ContactPerson)) updatedPublisher.ContactPerson = existingPublisher.ContactPerson;
                if (string.IsNullOrEmpty(updatedPublisher.StreetAddress)) updatedPublisher.StreetAddress = existingPublisher.StreetAddress;
                if (string.IsNullOrEmpty(updatedPublisher.City)) updatedPublisher.City = existingPublisher.City;
                if (string.IsNullOrEmpty(updatedPublisher.Region)) updatedPublisher.Region = existingPublisher.Region;
                if (string.IsNullOrEmpty(updatedPublisher.PostalCode)) updatedPublisher.PostalCode = existingPublisher.PostalCode;
                if (string.IsNullOrEmpty(updatedPublisher.Country)) updatedPublisher.Country = existingPublisher.Country;

                publisherRepository.Update(updatedPublisher);
                await publisherRepository.SaveChanges();
                break;
            case 3:
                existingPublisher = await Selection();

                await publisherRepository.Delete(existingPublisher.PublisherId);
                break;
        }
    }

    public static async Task<Publisher> Selection(bool fromOther = false)
    {
        if (fromOther) publisherRepository = new PublisherRepository(new BookStoreContext());
        
        var publishers = await publisherRepository.All() as List<Publisher>;
        List<string> publisherNames = publishers.Select(p => p.PublisherName).ToList();
        publisherNames.Add("Publisher name not available");
        int publisherChoice = BookStoreManager.SelectionMenu("Select Publisher",publisherNames);
        Publisher publisher;
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
            await publisherRepository.Add(publisher);
            await publisherRepository.SaveChanges();
        }
        else
        {
            publisher = publishers[publisherChoice];
        }

        return publisher;
    }
}