using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace codehacks_durable_function_demo
{
    public static class CreateOrderRecordFunction
    {
        [FunctionName("CreateOrderRecord")]
        public static void Run([ActivityTrigger] Order order, ILogger log)
        {
            // Create a record of the order in the database
            log.LogInformation($"Creating order record for order {order.OrderId}");
            // ...
        }
    }
}
