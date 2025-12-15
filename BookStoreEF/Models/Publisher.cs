using System;
using System.Collections.Generic;

namespace BookStoreEF.Models;

public partial class Publisher
{
    public int PublisherId { get; set; }

    public string PublisherName { get; set; } = null!;

    public string? EmailAddress { get; set; }

    public string? Website { get; set; }

    public string? PhoneNumber { get; set; }

    public string? ContactPerson { get; set; }

    public string? StreetAddress { get; set; }

    public string? City { get; set; }

    public string? Region { get; set; }

    public string? PostalCode { get; set; }

    public string Country { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public override string ToString()
    {
        return
            $"Name: {PublisherName}, E-mail address: {EmailAddress}, Website: {Website}, Phone Number: {PhoneNumber}, Contact Person: {ContactPerson}, Street Address: {StreetAddress}, City: {City}, Region: {Region}, Postal Code: {PostalCode}, Country: {Country}\n";
    }
}
