using System;
using System.Collections.Generic;

namespace ConsoleApp.Models;

public partial class Cfptype
{
    public int CfptypeId { get; set; }

    public string CfptypeName { get; set; } = null!;

    public string? CfptypeDescription { get; set; }

    public int SortOrder { get; set; }

    public bool IsEnabled { get; set; }

    public virtual ICollection<Cfp> Cfps { get; set; } = new List<Cfp>();
}
