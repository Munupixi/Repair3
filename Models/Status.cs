using System;
using System.Collections.Generic;

namespace Repair3.Models;

public partial class Status
{
    public int StatusId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<UserOrder> UserOrders { get; set; } = new List<UserOrder>();
}
