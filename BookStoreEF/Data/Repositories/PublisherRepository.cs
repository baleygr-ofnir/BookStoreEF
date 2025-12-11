using BookStoreEF.Data;
using BookStoreEF.Models;

namespace BookStoreEF.Management;

public class PublisherRepository : GenericRepository<Publisher>
{
    public PublisherRepository(BookStoreContext context) : base(context)
    {
    }

    public override Task<Publisher> Update(Publisher entity)
    {
        var publisher = Context.Publishers
            .FirstOrDefault(p => p.PublisherId == entity.PublisherId);

        publisher.PublisherName = entity.PublisherName;
        publisher.EmailAddress = entity.EmailAddress;
        publisher.Website = entity.Website;
        publisher.PhoneNumber = entity.PhoneNumber;
        publisher.ContactPerson = entity.ContactPerson;
        publisher.StreetAddress = entity.StreetAddress;
        publisher.City = entity.City;
        publisher.Region = entity.Region;
        publisher.PostalCode = entity.PostalCode;
        publisher.Country = entity.Country;

        return base.Update(entity);
    }
}