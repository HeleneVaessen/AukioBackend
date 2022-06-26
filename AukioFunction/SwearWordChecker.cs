using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AukioFunction
{
    public static class SwearWordChecker
    {
        [FunctionName("SwearWordChecker")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var client = new RestClient("https://api.apilayer.com/bad_words?censor_character=#");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            var request = new RestRequest();

            request.AddHeader("apikey", "dqCnCnej0R03I3BoxFLGThlQ1uFYFIjP");

            request.AddParameter("text/plain", name, ParameterType.RequestBody);

            var response = await client.ExecutePostAsync(request);

            var jsondata = (JObject)JsonConvert.DeserializeObject(response.Content);

            int swearAmount = jsondata["bad_words_total"].Value<int>();

            log.LogInformation("C# HTTP trigger function processed a request.");

            if (swearAmount > 0)
            {
                return new BadRequestObjectResult("Swear Words Detected");
            }

            return new OkObjectResult("No Swear Words Detected");
        }
    }
}
