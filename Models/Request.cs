using System;
using System.Collections.Generic;

namespace Repair3.Models;

public partial class Request
{
    public Request(int requestId, DateOnly creationDate, string? executorComment,
        int statusId, string? serviceType, string? faultType)
    {
        RequestId = requestId;
        CreationDate = creationDate;
        ExecutorComment = executorComment;
        StatusId = statusId;
        ServiceType = serviceType;
        FaultType = faultType;
    }
    public int RequestId { get; set; }

    public string? CompleteName { get; set; }

    public DateOnly CreationDate { get; set; }

    public string? ExecutorComment { get; set; }

    public int StatusId { get; set; }

    public string? ServiceType { get; set; }

    public string? FaultType { get; set; }

    public int? ReportId { get; set; }

    public int? ExecutorId { get; set; }

    public virtual User? Executor { get; set; }

    public virtual Report? Report { get; set; }

    public virtual Status Status { get; set; } = null!;

    public virtual ICollection<UserOrder> UserOrders { get; set; } = new List<UserOrder>();
}
