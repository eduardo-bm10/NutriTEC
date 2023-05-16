using System;
using System.Collections.Generic;

namespace Postgre_API;

public partial class Medida
{
    public string CedulaPaciente { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public double Cintura { get; set; }

    public double Cuello { get; set; }

    public double Caderas { get; set; }

    public double PorcentajeMusculo { get; set; }

    public double PorcentajeGrasa { get; set; }

    public virtual Paciente CedulaPacienteNavigation { get; set; } = null!;
}
