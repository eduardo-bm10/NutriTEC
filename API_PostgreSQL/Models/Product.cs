using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class Product
{
    public int Barcode { get; set; }

    public string Description { get; set; } = null!;

    public double Iron { get; set; }

    public double Sodium { get; set; }

    public double Energy { get; set; }

    public double Fat { get; set; }

    public double Calcium { get; set; }

    public double Carbohydrate { get; set; }

    public double Protein { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<AdminProductAssociation> AdminProductAssociations { get; set; } = new List<AdminProductAssociation>();

    public virtual ICollection<RecipeProductAssociation> RecipeProductAssociations { get; set; } = new List<RecipeProductAssociation>();

    public virtual ICollection<Vitamin> Vitamins { get; set; } = new List<Vitamin>();

    public virtual ICollection<MealTime> Mealtimes { get; set; } = new List<MealTime>();
}
