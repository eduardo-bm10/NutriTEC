using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class Vitamin
{
    public int ProductBarcode { get; set; }

    public string Vitamin1 { get; set; } = null!;

    public virtual Product ProductBarcodeNavigation { get; set; } = null!;
}
