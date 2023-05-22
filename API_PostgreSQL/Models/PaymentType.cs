using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class PaymentType
{
    public int Id { get; set; }

    public string Nutritionistid { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual Nutritionist Nutritionist { get; set; } = null!;
}
