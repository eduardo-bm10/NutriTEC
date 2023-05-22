using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class Plan
{
    public int Id { get; set; }

    public string Nutritionistid { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual Nutritionist Nutritionist { get; set; } = null!;

    public virtual ICollection<PlanMealtimeAssociation> PlanMealtimeAssociationMealtimes { get; set; } = new List<PlanMealtimeAssociation>();

    public virtual ICollection<PlanMealtimeAssociation> PlanMealtimeAssociationPlans { get; set; } = new List<PlanMealtimeAssociation>();

    public virtual ICollection<PlanPatientAssociation> PlanPatientAssociations { get; set; } = new List<PlanPatientAssociation>();
}
