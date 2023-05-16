using System;
using System.Collections.Generic;

namespace Postgre_API;

public partial class Producto
{
    public int CodigoBarras { get; set; }

    public string Descripcion { get; set; } = null!;

    public double Hierro { get; set; }

    public double Sodio { get; set; }

    public double Energia { get; set; }

    public double Grasa { get; set; }

    public double Calcio { get; set; }

    public double Carbohidrato { get; set; }

    public double Proteina { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<AsociacionRecetaProducto> AsociacionRecetaProductos { get; set; } = new List<AsociacionRecetaProducto>();

    public virtual ICollection<Administrador> CedulaAdmins { get; set; } = new List<Administrador>();
}
