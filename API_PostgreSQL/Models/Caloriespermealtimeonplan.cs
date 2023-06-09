using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class Caloriespermealtimeonplan
{
    public int? Planid { get; set; }

    public string? Mealtime { get; set; }

    public double? Totalcalories { get; set; }
}
