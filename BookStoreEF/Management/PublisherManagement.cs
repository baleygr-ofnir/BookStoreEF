using BookStoreEF.Data;
using BookStoreEF.Models;

namespace BookStoreEF.Management;

public static class PublisherManagement
{
    private static IRepository<Publisher> publisherRepository;

    private static List<string> menuOptions = new()
    {
        "Add new publisher",
        "List all publishers",
        "Update existing publisher",
        "Delete existing publisher",
    };

    public static async Task Open(BookStoreContext context)
    {
        publisherRepository = new PublisherRepository(context);
        int menuChoice = BookStoreManager.SelectionMenu("Publisher", menuOptions);
        var publishers = await publisherRepository.All() as List<Publisher>;
        List<string> publisherNames = publishers.Select(p => p.PublisherName) as List<string>;
        int publisherChoice;
        Publisher publisher;
        
        switch (menuChoice)
        {
            case 0:
                publisher = SetupNew();
                await publisherRepository.Add(publisher);
                await publisherRepository.SaveChanges();
                break;
            case 1:
                Console.WriteLine("Stored publishers:");
                publishers.ForEach(Console.WriteLine);
                break;
            case 2:
                publisherChoice = BookStoreManager.SelectionMenu("Select Publisher", publisherNames);
                publisher = publishers[publisherChoice];
                Publisher updatedPublisher = SetupNew(true);
                updatedPublisher.PublisherId = publisher.PublisherId;

                if (string.IsNullOrEmpty(updatedPublisher.PublisherName)) updatedPublisher.PublisherName = publisher.PublisherName;
                if (string.IsNullOrEmpty(updatedPublisher.EmailAddress)) updatedPublisher.EmailAddress = publisher.EmailAddress;
                if (string.IsNullOrEmpty(updatedPublisher.Website)) updatedPublisher.Website = publisher.Website;
                if (string.IsNullOrEmpty(updatedPublisher.PhoneNumber)) updatedPublisher.PhoneNumber = publisher.PhoneNumber;
                if (string.IsNullOrEmpty(updatedPublisher.ContactPerson)) updatedPublisher.ContactPerson = publisher.ContactPerson;
                if (string.IsNullOrEmpty(updatedPublisher.StreetAddress)) updatedPublisher.StreetAddress = publisher.StreetAddress;
                if (string.IsNullOrEmpty(updatedPublisher.City)) updatedPublisher.City = publisher.City;
                if (string.IsNullOrEmpty(updatedPublisher.Region)) updatedPublisher.Region = publisher.Region;
                if (string.IsNullOrEmpty(updatedPublisher.PostalCode)) updatedPublisher.PostalCode = publisher.PostalCode;
                if (string.IsNullOrEmpty(updatedPublisher.Country)) updatedPublisher.Country = publisher.Country;
                
                publisherRepository.Update(updatedPublisher);
                await publisherRepository.SaveChanges();
                break;
            case 3:
                publisherChoice = BookStoreManager.SelectionMenu("Select Publisher", publisherNames);
                publisher = publishers[publisherChoice];

                await publisherRepository.Delete(publisher.PublisherId);
                break;
        }
    }

    public static Publisher SetupNew(bool updating = false)
    {
        if (updating) Console.WriteLine("Enter nothing for anything to leave unchanged...");
        
        Console.Write("Enter publisher name");
        string name = Console.ReadLine();
        
        Console.Write("Enter e-mail address: ");
        string email = Console.ReadLine();
        
        Console.Write("Enter link to website");
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
}