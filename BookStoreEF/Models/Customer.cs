using System;
using System.Collections.Generic;

namespace BookStoreEF.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? StreetAddress { get; set; }

    public string? City { get; set; }

    public string? Region { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public DateOnly? BirthDate { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
