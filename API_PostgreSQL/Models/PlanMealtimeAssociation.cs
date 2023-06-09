using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class PlanMealtimeAssociation
{
    public int Planid { get; set; }

    public int Mealtimeid { get; set; }

    public int Productbarcode { get; set; }

    public int Filler { get; set; }

    public virtual Plan Mealtime { get; set; } = null!;

    public virtual Plan Plan { get; set; } = null!;

    public virtual Product ProductbarcodeNavigation { get; set; } = null!;
}
