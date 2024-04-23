using ExerciceRecrutementRestaurant.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciceRecrutementRestaurant.Services
{
    public class RestaurantService
    {
        public RestaurantService() { }

        public List<Restaurant> GetAllRestaurants()
        {
            List<Restaurant> restaurants;
            using (var db = new RestaurantsContext())
            {

                restaurants = db.Restaurants.ToList();

            }

            return restaurants;

        }

        public List<Meal> GetRestaurantMeals(string restaurantId)
        {
            Restaurant restaurant;
            using (var db = new RestaurantsContext())
            {
                restaurant = db.Restaurants.Include(r => r.Meals).ToList().Find(elem => elem.Id.Equals(restaurantId));
            }

            if (restaurant == null)
            {
                return new List<Meal>();
            }

            List<Meal> meals = new List<Meal>();
            foreach (Meal meal in restaurant.Meals)
            {
                meals.Add(meal);
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

            try
            {
                mealService.AddMeal(meal);
            }
            catch (Exception exception)
            {
                // The meal already exists in DB, but that's ok
            }

            using (var db = new RestaurantsContext())
            {
                Restaurant restaurant = db.Restaurants.Include(r => r.Meals).ToList().Find(elem => elem.Id.Equals(restaurantId));

                Meal meal1 = db.Meals.FirstOrDefault(elem => elem.Id.Equals(meal.Id));

                restaurant.Meals.Add(meal1);

                db.Restaurants.Update(restaurant);
                db.SaveChanges();
            }

            return 0;

        }

        public int AddNewRestaurant(string name)
        {
            Restaurant restaurant = new Restaurant() {
                Id = System.Guid.NewGuid().ToString(),
                Name = name,
                Meals = new List<Meal>()
            };

            using (var db = new RestaurantsContext())
            {
                db.Restaurants.Add(restaurant);
                db.SaveChanges();
            }

            return 0;

        }

    }
}
