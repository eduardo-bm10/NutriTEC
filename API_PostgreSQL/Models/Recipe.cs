using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class Recipe
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<RecipeProductAssociation> RecipeProductAssociations { get; set; } = new List<RecipeProductAssociation>();
}
