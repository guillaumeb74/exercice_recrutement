using ExerciceRecrutementRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciceRecrutementRestaurant.Services
{
    public class RestaurantService
    {

        private List<Restaurant> restaurants = new List<Restaurant>()
        {
            new Restaurant()
            {
                Id = "0",
                Name = "Restaurant1",
                Meals = new List<string>()
                {
                    "0",
                    "1"
                }
            },
            new Restaurant()
            {
                Id = "1",
                Name = "Restaurant2",
                Meals = new List<string>()
                {
                    "2",
                    "3"
                }
            }
        };

        public RestaurantService() { }

        public List<Restaurant> GetAllRestaurants()
        {

            List<Restaurant> restaurants = this.restaurants;

            return restaurants;

        }

        public List<Meal> GetRestaurantMeals(string restaurantId)
        {
            MealService mealService = new MealService();

            List<Meal> meals = new List<Meal>();

            Restaurant restaurant = this.restaurants.Find(elem => elem.Id.Equals(restaurantId));
            foreach (string mealId in restaurant.Meals)
            {
                meals.Add(mealService.GetMeal(mealId));
            }

            return meals;

        }

        public int AddNewMealToRestaurant(string restaurantId, string mealName)
        {
            MealService mealService = new MealService();

            Meal meal = mealService.FindMealByName(mealName);
            if (meal.Name.Length <= 0)
            {
                return -1;
            }

            mealService.AddMeal(meal);

            Restaurant restaurant = this.restaurants.Find(elem => elem.Id.Equals(restaurantId));
            restaurant.Meals.Add(meal.Id);

            return 0;

        }

    }
}
