using ExerciceRecrutementRestaurant.Models;
using ExerciceRecrutementRestaurant.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.ServiceProcess;
using System.Xml.Linq;

namespace ExerciceRecrutementRestaurant.Controllers
{
    // The callable API for the loadbalancer
    [ApiController]
    [Route("api/Restaurant")]
    [Produces("application/json")]
    public class LoadBalancerController : ControllerBase
    {
        private string appKey = "fv5s4<3F5vdfe5wee-s";

        private RestaurantService restaurantService;

        public LoadBalancerController()
        {
            this.restaurantService = new RestaurantService();
        }

        [HttpGet]
        public IActionResult GetAllRestaurants()
        {
            try
            {
                List<Restaurant> restaurants = restaurantService.GetAllRestaurants();

                // The HTTP response
                OkObjectResult response = new OkObjectResult(null);
                response = Ok(restaurants);
                return response;
            }
            catch (Exception exception)
            {
                return new ObjectResult(exception.Message) { StatusCode = 500 };
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetRestaurantMeals(string id)
        {
            try
            {
                List<Meal> meals = restaurantService.GetRestaurantMeals(id);

                // The HTTP response
                OkObjectResult response = new OkObjectResult(null);
                response = Ok(meals);
                return response;
            }
            catch (Exception exception)
            {
                return new ObjectResult(exception.Message) { StatusCode = 500 };
            }
        }

        

        /// <summary>Make the DCS traitement on specified pdf file using specified json config.</summary>
        /// <param name="requestPostModel">  The json request.</param>
        /// <returns>IActionResult json response with code 200 if all went well or error code if there was a problem.</returns>
        [HttpPost("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Dictionary<string, object>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status419AuthenticationTimeout)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> Process(RequestPostModel requestPostModel) // -> If there is an async need in the method, use this instead
        public IActionResult AddNewMealToRestaurant(string id, JObject requestBody) // -> Sync version
        {
            try
            {
                // Find provided app key token in cookies
                /*
                List<string> cookies = this.HttpContext.Request.Headers.Cookie.ToList();
                string appKeyToken = cookies.Find(elem => elem.Contains("access_token="));
                appKeyToken = appKeyToken.Replace("access_token=", "");
                */

                // Check if the provided app key is valid
                /*if (appKeyToken is null || !appKeyToken.Equals(this.appKey))
                {
                    return BadRequest(new Dictionary<string, object> {
                        { "Status", "Error" },
                        { "Message", "The provided app key is invalid" }
                    });
                }*/

                string mealToAdd = requestBody["meal"].ToString();

                int success = restaurantService.AddNewMealToRestaurant(id, mealToAdd);

                if (success != 0)
                {
                    return BadRequest(new Dictionary<string, object> {
                        { "Status", "Error" },
                        { "Message", "The provided meal is invalid" }
                    });
                }

                // On renvoie un message de succes pour confirmer que le resultat a ete recu correctement
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
