using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class AdminProductAssociation
{
    public string Adminid { get; set; } = null!;

    public int Productbarcode { get; set; }

    public int Filler { get; set; }

    public virtual Administrator Admin { get; set; } = null!;

    public virtual Product ProductbarcodeNavigation { get; set; } = null!;
}
