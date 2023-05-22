using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class MealTime
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Consumption> Consumptions { get; set; } = new List<Consumption>();
}
