using System;
using System.Collections.Generic;

namespace Repair3.Models;

public partial class Part
{
    public int PartId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public float Cost { get; set; }

    public int QuantityInStock { get; set; }

    public virtual ICollection<UserOrder> UserOrders { get; set; } = new List<UserOrder>();
}
