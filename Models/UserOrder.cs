using System;
using System.Collections.Generic;

namespace Repair3.Models;

public partial class UserOrder
{
    public int UserOrderId { get; set; }

    public string? Priority { get; set; }

    public DateOnly CreationDate { get; set; }

    public int RequestId { get; set; }

    public int StatusId { get; set; }

    public virtual Request Request { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual ICollection<Part> Parts { get; set; } = new List<Part>();
}
