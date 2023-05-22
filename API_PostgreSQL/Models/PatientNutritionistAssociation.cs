using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class PatientNutritionistAssociation
{
    public string Nutritionistid { get; set; } = null!;

    public string Patientid { get; set; } = null!;

    public int Filler { get; set; }

    public virtual Nutritionist Nutritionist { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
