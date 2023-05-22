using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class Measurement
{
    public string Patientid { get; set; } = null!;

    public DateOnly Date { get; set; }

    public double Waist { get; set; }

    public double Neck { get; set; }

    public double Hips { get; set; }

    public double Musclepercentage { get; set; }

    public double Fatpercentage { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}
