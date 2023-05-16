using System;
using System.Collections.Generic;

namespace Postgre_API;

public partial class Recetum
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public string Porciones { get; set; } = null!;

    public virtual ICollection<AsociacionRecetaProducto> AsociacionRecetaProductos { get; set; } = new List<AsociacionRecetaProducto>();
}
