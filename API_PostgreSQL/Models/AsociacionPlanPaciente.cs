using System;
using System.Collections.Generic;

namespace Postgre_API;

public partial class AsociacionPlanPaciente
{
    public string CedulaPaciente { get; set; } = null!;

    public int IdPlan { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public virtual Paciente CedulaPacienteNavigation { get; set; } = null!;

    public virtual Plan IdPlanNavigation { get; set; } = null!;
}
