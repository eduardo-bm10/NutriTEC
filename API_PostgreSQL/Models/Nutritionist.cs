using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class Nutritionist
{
    public string Id { get; set; } = null!;

    public string? Nutritionistcode { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname1 { get; set; } = null!;

    public string Lastname2 { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Weight { get; set; }

    public double Bmi { get; set; }

    public string Address { get; set; } = null!;

    public byte[] Photo { get; set; } = null!;

    public int Paymentid { get; set; }

    public virtual ICollection<PatientNutritionistAssociation> PatientNutritionistAssociations { get; set; } = new List<PatientNutritionistAssociation>();

    public virtual PaymentType Payment { get; set; } = null!;

    public virtual ICollection<Plan> Plans { get; set; } = new List<Plan>();
}
