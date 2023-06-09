using System;
using System.Collections.Generic;

namespace Postgre_API.Models;

public partial class Totalrecipecalory
{
    public int? Recipeid { get; set; }

    public string? Recipedescription { get; set; }

    public string? Product { get; set; }

    public double? Productcalories { get; set; }

    public int? Productportion { get; set; }

    public double? Totalproductcalories { get; set; }

    public double? Totalrecipecalories { get; set; }
}
