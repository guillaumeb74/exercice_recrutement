using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExerciceRecrutementRestaurant.Services
{
    // Class used to call the MEAL DB API to retrieve some meal data
    public class MealApiService
    {
        public MealApiService() { }

        public async Task<JObject> FindMealInApiByName(string name)
        {
            // Accept https bad certificate - useful in test environment, to disable for production !
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            // Create httpclient
            HttpClient client = new HttpClient(handler);

            string url = "https://www.themealdb.com/api/json/v1/1/search.php?s=" + name;

            // Call http client using specified method
            HttpResponseMessage response = await client.GetAsync(url);

            var result = await response.Content.ReadAsStringAsync();

            // Check if response is succesful or not
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new Exception(response.StatusCode.ToString() + " " + result);
            }
            response.EnsureSuccessStatusCode();
            // Deserialize the updated product from the response body.

            JObject resultObject = JObject.Parse(result);
            return resultObject;
        }

    }
}
