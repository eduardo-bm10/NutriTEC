using System;
using System.Collections.Generic;

namespace Postgre_API;

public partial class Consumo
{
    public string CedulaPaciente { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public int TiempoComida { get; set; }

    public virtual Paciente CedulaPacienteNavigation { get; set; } = null!;

    public virtual TiempoComidum TiempoComidaNavigation { get; set; } = null!;
}
