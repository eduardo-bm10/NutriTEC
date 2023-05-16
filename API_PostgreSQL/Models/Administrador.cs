using System;
using System.Collections.Generic;

namespace Postgre_API;

public partial class Administrador
{
    public string Cedula { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido1 { get; set; } = null!;

    public string Apellido2 { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public virtual ICollection<Producto> CodigoBarrasProductos { get; set; } = new List<Producto>();
}
