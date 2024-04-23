using ExerciceRecrutementRestaurant.Models;
using ExerciceRecrutementRestaurant.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.ServiceProcess;

namespace ExerciceRecrutementRestaurant.Controllers
{
    // The callable API for the meal model
    [ApiController]
    [Route("api/Meal")]
    [Produces("application/json")]
    public class MealController : ControllerBase
    {
        private MealService mealService;

        public MealController()
        {
            this.mealService = new MealService();
        }

        // GET REQUEST TO FIND A MEAL IN THE API BY NAME
        [HttpGet("{name}")]
        public IActionResult FindMealByName(string name)
        {
            try
            {
                Meal meal = mealService.FindMealByName(name);
                // The HTTP response
                OkObjectResult response = new OkObjectResult(null);
                response = Ok(meal);
                return response;
            }
            catch (Exception exception)
            {
                return new ObjectResult(exception.Message) { StatusCode = 500 };
            }
        }

    }
}
