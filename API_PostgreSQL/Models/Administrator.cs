using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class Administrator
{
    public string Id { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname1 { get; set; } = null!;

    public string Lastname2 { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<AdminProductAssociation> AdminProductAssociations { get; set; } = new List<AdminProductAssociation>();
}
