using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class Patient
{
    public string Id { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname1 { get; set; } = null!;

    public string Lastname2 { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Weight { get; set; }

    public double Bmi { get; set; }

    public string Address { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public string Country { get; set; } = null!;

    public double Maxconsumption { get; set; }

    public virtual Consumption? Consumption { get; set; }

    public virtual Measurement? Measurement { get; set; }

    public virtual ICollection<PatientNutritionistAssociation> PatientNutritionistAssociations { get; set; } = new List<PatientNutritionistAssociation>();

    public virtual ICollection<PlanPatientAssociation> PlanPatientAssociations { get; set; } = new List<PlanPatientAssociation>();
}
