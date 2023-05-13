using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace codehacks_durable_function_demo
{
    public static class SendOrderNotificationFunction
    {
        [FunctionName("SendOrderNotification")]
        public static void Run([ActivityTrigger] Order order, ILogger log)
        {
            // Send a notification to the customer that the order has been received
            log.LogInformation($"Sending notification to customer for order {order.OrderId}");
            // ...
        }
    }
}
