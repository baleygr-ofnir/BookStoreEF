using System;
using System.Collections.Generic;

namespace BookStoreEF.Models;

public partial class Author
{
    public int AuthorId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? Birthdate { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    
    public override string ToString()
    {
        return $"Name: {FirstName} {LastName}, Birthdate: {Birthdate}\n";
    }
}
