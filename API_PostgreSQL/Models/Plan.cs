using System;
using System.Collections.Generic;

namespace Postgre_API;

public partial class Plan
{
    public int Id { get; set; }

    public string CedulaNutri { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<AsociacionPlanPaciente> AsociacionPlanPacientes { get; set; } = new List<AsociacionPlanPaciente>();

    public virtual Nutricionistum CedulaNutriNavigation { get; set; } = null!;

    public virtual ICollection<Plan> IdPlans { get; set; } = new List<Plan>();

    public virtual ICollection<Plan> IdTiempoComida { get; set; } = new List<Plan>();
}
