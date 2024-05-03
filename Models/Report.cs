using System;
using System.Collections.Generic;

namespace Repair3.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public string? ProvidedAssistance { get; set; }

    public string? MainfactionCause { get; set; }

    public string? MaterualsSpend { get; set; }

    public float? MoneySpend { get; set; }

    public float? TimeSpend { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
