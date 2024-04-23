using ExerciceRecrutementRestaurant.Models;
using ExerciceRecrutementRestaurant.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.ServiceProcess;

namespace ExerciceRecrutementRestaurant.Controllers
{
    // The callable API for the loadbalancer
    [ApiController]
    [Route("api/Meal")]
    [Produces("application/json")]
    public class MealController : ControllerBase
    {
        private string appKey = "fv5s4<3F5vdfe5wee-s";

        private MealService mealService;

        public MealController()
        {
            this.mealService = new MealService();
        }

        [HttpGet("{name}")]
        public IActionResult FindMealByName(string name)
        {
            try
            {
                //Meal meal = new Meal();
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
