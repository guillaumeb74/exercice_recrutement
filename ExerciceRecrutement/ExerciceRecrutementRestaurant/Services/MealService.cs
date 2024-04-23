using ExerciceRecrutementRestaurant.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExerciceRecrutementRestaurant.Services
{
    public class MealService
    {

        private List<Meal> meals = new List<Meal>()
        {
            new Meal()
            {
                Id = "0",
                Name = "meal1",
            },
            new Meal()
            {
                Id = "1",
                Name = "meal2",
            },
            new Meal()
            {
                Id = "2",
                Name = "meal3",
            },
            new Meal()
            {
                Id = "3",
                Name = "meal4",
            }
        };

        public MealService() { }

        public Meal GetMeal(string mealId)
        {

            Meal meal = this.meals.Find(elem => elem.Id.Equals(mealId));

            return meal;

        }

        // Find a meal in Meal DB API matching name
        public Meal FindMealByName(string name)
        {

            JObject result = ExecuteFindMealInApiByName(name).GetAwaiter().GetResult();

            string mealId = (result["meals"][0])["idMeal"].ToString();
            string mealName = (result["meals"][0])["strMeal"].ToString();
            string mealImage = (result["meals"][0])["strMealThumb"].ToString();

            Meal meal = new Meal()
            {
                Id = mealId,
                Name = mealName,
                Image = mealImage,
            };

            return meal;
        }

        // Launch the task in a new thread and wait for a result
        public static async Task<JObject> ExecuteFindMealInApiByName(string name)
        {
            try
            {
                MealApiService mealApiService = new MealApiService();

                Task<JObject> task = Task.Run(() =>
                    mealApiService.FindMealInApiByName(name).GetAwaiter().GetResult()
                );
                JObject result = await task;

                return result;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void AddMeal(Meal meal)
        {
            this.meals.Add(meal);
        }

    }
}
