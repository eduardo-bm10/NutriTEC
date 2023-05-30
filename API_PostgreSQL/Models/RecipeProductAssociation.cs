using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class RecipeProductAssociation
{
    public int Recipeid { get; set; }

    public int Productbarcode { get; set; }

    public int Productportion { get; set; }

    public int Filler { get; set; }

    public virtual Product ProductbarcodeNavigation { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
