using System;
using System.Collections.Generic;

namespace Postgre_API;

public partial class AsociacionRecetaProducto
{
    public int IdReceta { get; set; }

    public int CodigoBarrasProducto { get; set; }

    public int PorcionProducto { get; set; }

    public virtual Producto CodigoBarrasProductoNavigation { get; set; } = null!;

    public virtual Recetum IdRecetaNavigation { get; set; } = null!;
}
