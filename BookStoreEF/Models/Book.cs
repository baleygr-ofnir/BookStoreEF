using System;
using System.Collections.Generic;

namespace BookStoreEF.Models;

public partial class Book
{
    public string Isbn { get; set; } = null!;

    public int AuthorId { get; set; }

    public string Title { get; set; } = null!;

    public string Language { get; set; } = null!;

    public decimal Price { get; set; }

    public DateOnly? PublicationDate { get; set; }

    public int PublisherId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual Publisher Publisher { get; set; } = null!;
    
    public override string ToString()
    {
        return
            $"Title: {Title}, ISBN: {Isbn}, Author: {Author.FirstName} {Author.LastName}, Language: {Language}, Price: {Price}, Publication Date: {PublicationDate}, Publisher: {Publisher.PublisherName}\n";
    }
}
