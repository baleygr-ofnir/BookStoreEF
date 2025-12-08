using BookStoreEF.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEF.Data;

public class CustomerDbService(BookStoreContext context)
{
    private BookStoreContext _context;
    
    // Customer Actions

    public async Task<Customer> CreateCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<List<Customer>> GetCustomers()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer> GetCustomer(int id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task<List<Order>> GetCustomerOrders(int customerId)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(order => order.CustomerId == customerId)
            .Include(order => order.OrderDetails)
            .ThenInclude(orderDetails => orderDetails.IsbnNavigation)
            .Include(order => order.Store)
            .OrderByDescending(order => order.OrderDate)
            .ToListAsync();
    }
    
    public async Task<Customer> UpdateCustomer(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<bool> DeleteCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return false;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return true;
    }
}