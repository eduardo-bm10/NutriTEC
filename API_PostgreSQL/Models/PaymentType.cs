using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class PaymentType
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Nutritionist> Nutritionists { get; set; } = new List<Nutritionist>();
}
