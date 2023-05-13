using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace codehacks_durable_function_demo
{
    public static class FulfillShippingFunction
    {
        [FunctionName("FulfillShipping")]
        public static void Run([ActivityTrigger] Order order, ILogger log)
        {
            // Fulfill the order and ship it to the customer
            log.LogInformation($"Fulfilling shipping for order {order.OrderId}");
            // ...
        }
    }
}
