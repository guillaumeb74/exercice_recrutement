﻿using ExerciceRecrutementRestaurant.Models;
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

        public MealService() { }

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

        // Add a new meal in DB
        public void AddMeal(Meal meal)
        {
            using (var db = new RestaurantsContext())
            {
                db.Meals.Add(meal);
                db.SaveChanges();
            }
        }

    }
}
