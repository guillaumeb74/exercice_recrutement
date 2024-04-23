using ExerciceRecrutementRestaurant.Models;
using ExerciceRecrutementRestaurant.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.ServiceProcess;
using System.Xml.Linq;

namespace ExerciceRecrutementRestaurant.Controllers
{
    // The callable API for the restaurant model
    [ApiController]
    [Route("api/Restaurant")]
    [Produces("application/json")]
    public class RestaurantController : ControllerBase
    {
        private RestaurantService restaurantService;

        public RestaurantController()
        {
            this.restaurantService = new RestaurantService();
        }

        // GET REQUEST TO RETRIEVE ALL RESTAURANTS
        [HttpGet]
        public IActionResult GetAllRestaurants()
        {
            try
            {
                List<Restaurant> restaurants = restaurantService.GetAllRestaurants();
                // The HTTP response
                OkObjectResult response = new OkObjectResult(null);
                // Return an anonymous object with only the needed properties
                var formattedRestaurants = restaurants.Select(restaurant => new { restaurant.Id, restaurant.Name });
                response = Ok(formattedRestaurants);
                return response;
            }
            catch (Exception exception)
            {
                return new ObjectResult(exception.Message) { StatusCode = 500 };
            }
        }

        // GET REQUEST TO RETRIEVE ALL THE MEALS FOR A SPECIFIC RESTAURANT
        [HttpGet("{id}")]
        public IActionResult GetRestaurantMeals(string id)
        {
            try
            {
                List<Meal> meals = restaurantService.GetRestaurantMeals(id);
                // The HTTP response
                OkObjectResult response = new OkObjectResult(null);
                // Return an anonymous object with only the needed properties
                var formattedMeals = meals.Select(meal => new { meal.Id, meal.Name, meal.Image });
                response = Ok(formattedMeals);
                return response;
            }
            catch (Exception exception)
            {
                return new ObjectResult(exception.Message) { StatusCode = 500 };
            }
        }

        // POST REQUEST TO ADD A NEW MEAL TO A SPECIFIC RESTAURANT
        [HttpPost("{id}")]
        public IActionResult AddNewMealToRestaurant(string id, JObject requestBody)
        {
            try
            {
                string mealToAdd = requestBody["meal"].ToString();
                int success = restaurantService.AddNewMealToRestaurant(id, mealToAdd);
                if (success != 0)
                {
                    return BadRequest(new Dictionary<string, object> {
                        { "Status", "Error" },
                        { "Message", "The provided meal / restaurant is invalid" }
                    });
                }
                // The HTTP response
                OkObjectResult response = new OkObjectResult(null);
                response = Ok(new Dictionary<string, object> {
                    { "Status", "Success" }
                });
                return response;
            }
            catch (Exception e)
            {
                return new ObjectResult(e.Message) { StatusCode = 500 };
            }
        }

        // POST REQUEST TO ADD A NEW RESTAURANT
        [HttpPost]
        public IActionResult AddNewRestaurant(JObject requestBody)
        {
            try
            {
                string name = requestBody["name"].ToString();
                int success = restaurantService.AddNewRestaurant(name);
                if (success != 0)
                {
                    return BadRequest(new Dictionary<string, object> {
                        { "Status", "Error" },
                        { "Message", "The provided restaurant is invalid" }
                    });
                }
                // The HTTP response
                OkObjectResult response = new OkObjectResult(null);
                response = Ok(new Dictionary<string, object> {
                    { "Status", "Success" }
                });

                return response;
            }
            catch (Exception e)
            {
                return new ObjectResult(e.Message) { StatusCode = 500 };
            }
        }

    }
}
