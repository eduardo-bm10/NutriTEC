using System;
using System.Collections.Generic;

namespace Postgre_API;

public partial class Nutricionistum
{
    public string Cedula { get; set; } = null!;

    public string? CodigoNutricionista { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido1 { get; set; } = null!;

    public string Apellido2 { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public int Peso { get; set; }

    public double Imc { get; set; }

    public string Direccion { get; set; } = null!;

    public byte[] Foto { get; set; } = null!;

    public virtual Plan? Plan { get; set; }

    public virtual ICollection<TipoCobro> TipoCobros { get; set; } = new List<TipoCobro>();

    public virtual ICollection<Paciente> CedulaPacientes { get; set; } = new List<Paciente>();
}
