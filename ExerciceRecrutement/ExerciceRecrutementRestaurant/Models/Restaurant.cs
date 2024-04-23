
using System.Runtime.Serialization;

namespace ExerciceRecrutementRestaurant.Models
{
    public class Restaurant
    {

        public string Id { get; set; }

        public string Name { get; set; }

        [IgnoreDataMemberAttribute]
        public List<string> Meals { get; set; }

        public Restaurant() { }

    }
}
