using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class Consumption
{
    public string Patientid { get; set; } = null!;

    public DateOnly Date { get; set; }

    public int Mealtime { get; set; }

    public virtual MealTime MealtimeNavigation { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
