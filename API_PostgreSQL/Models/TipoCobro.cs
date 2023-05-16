using System;
using System.Collections.Generic;

namespace Postgre_API;

public partial class TipoCobro
{
    public int Id { get; set; }

    public string CedulaNutri { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual Nutricionistum CedulaNutriNavigation { get; set; } = null!;
}
