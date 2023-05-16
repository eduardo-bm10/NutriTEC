using System;
using System.Collections.Generic;

namespace Postgre_API;

public partial class Paciente
{
    public string Cedula { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido1 { get; set; } = null!;

    public string Apellido2 { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public int Peso { get; set; }

    public double Imc { get; set; }

    public string Direccion { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string Pais { get; set; } = null!;

    public double ConsumoMaximo { get; set; }

    public virtual ICollection<AsociacionPlanPaciente> AsociacionPlanPacientes { get; set; } = new List<AsociacionPlanPaciente>();

    public virtual Consumo? Consumo { get; set; }

    public virtual Medida? Medida { get; set; }

    public virtual ICollection<Nutricionistum> CedulaNutris { get; set; } = new List<Nutricionistum>();
}
