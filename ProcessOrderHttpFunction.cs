using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace codehacks_durable_function_demo
{
    /*These functions perform the individual activities that the `ProcessOrder` function calls. 
     * They take an `Order` object as input, perform their respective tasks, and log the results.To run the `ProcessOrder` 
     * function, you'll need to trigger it using an HTTP request or other trigger mechanism supported by Azure Functions. 
     * For example, you can add an HTTP trigger function to the same project that calls the `ProcessOrder` function, 
     * like this:*/

    public static class ProcessOrderHttpFunction
    {
        [FunctionName("ProcessOrderHttp")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient client,
            ILogger log)
        {
            // Parse the order information from the request body
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var order = JsonConvert.DeserializeObject<Order>(requestBody);

            // Start the durable function to process the order
            var instanceId = await client.StartNewAsync("ProcessOrder", order);

            // Return the instance ID to the client
            return new OkObjectResult(new { instanceId });
        }
    }
}
