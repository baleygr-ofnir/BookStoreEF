using BookStoreEF.Data;
using BookStoreEF.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEF.Management;

public class CustomerRepository : GenericRepository<Customer>
{
    public CustomerRepository(BookStoreContext context) : base(context)
    {
    }

    public override Task<Customer> Update(Customer entity)
    {
        var customer = Context.Customers.FirstOrDefault(c => c.CustomerId == entity.CustomerId);

        customer.FirstName = entity.FirstName;
        customer.LastName = entity.LastName;
        customer.EmailAddress = entity.EmailAddress;
        customer.PhoneNumber = entity.PhoneNumber;
        customer.StreetAddress = entity.StreetAddress;
        customer.City = entity.City;
        customer.Region = entity.Region;
        customer.PostalCode = entity.PostalCode;
        customer.Country = entity.Country;
        customer.BirthDate = entity.BirthDate;
        customer.CreationDate = entity.CreationDate;
        customer.ModifiedDate = DateTime.Now;

        return base.Update(customer);
    }
}