using System;
using System.Collections.Generic;

namespace Postgre_API;

public partial class TiempoComidum
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Consumo> Consumos { get; set; } = new List<Consumo>();
}
