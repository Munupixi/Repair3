using System;
using System.Collections.Generic;
using Repair3.Models;

namespace Repair3;

public partial class Status
{
    public int StatusId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual ICollection<UserOrder> UserOrders { get; set; } = new List<UserOrder>();
}
