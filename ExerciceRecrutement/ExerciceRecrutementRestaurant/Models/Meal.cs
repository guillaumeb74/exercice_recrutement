using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ExerciceRecrutementRestaurant.Models;

public partial class Meal
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? Image { get; set; }

    //[IgnoreDataMemberAttribute]
    public virtual ICollection<Restaurant> Restaurants { get; set; }

}
