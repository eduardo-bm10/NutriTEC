using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class PlanPatientAssociation
{
    public string Patientid { get; set; } = null!;

    public int Planid { get; set; }

    public DateOnly Startdate { get; set; }

    public DateOnly Enddate { get; set; }

    public int Filler { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual Plan Plan { get; set; } = null!;
}
