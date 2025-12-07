using System;
using System.Collections.Generic;

namespace BookStoreEF.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public string Isbn { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? Discount { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual Book IsbnNavigation { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
