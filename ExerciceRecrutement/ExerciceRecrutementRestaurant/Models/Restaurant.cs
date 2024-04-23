using System;
using System.Collections.Generic;

namespace ExerciceRecrutementRestaurant.Models;

public partial class Restaurant
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public virtual ICollection<Meal> Meals { get; set; }

}
