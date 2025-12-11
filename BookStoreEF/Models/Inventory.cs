using System;
using System.Collections.Generic;

namespace BookStoreEF.Models;

public partial class Inventory
{
    public int StoreId { get; set; }

    public string Isbn { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual Book IsbnNavigation { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;

    public override string ToString()
    {
        return
            $"Store: {Store} ({StoreId}), Book: {IsbnNavigation.Title} ({Isbn}), Price: {IsbnNavigation.Price}, Quantity: {Quantity}";
    }
}
