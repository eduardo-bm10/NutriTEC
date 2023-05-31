using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class MealtimeProduct
{
    public int Mealtimeid { get; set; }

    public int ProductBarcode { get; set; }

    public int Filler { get; set; }

    public virtual MealTime Mealtime { get; set; } = null!;

    public virtual Product ProductBarcodeNavigation { get; set; } = null!;
}
