using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace codehacks_durable_function_demo
{
    public static class ProcessPaymentFunction
    {
        [FunctionName("ProcessPayment")]
        public static void Run([ActivityTrigger] Order order, ILogger log)
        {
            // Process the payment for the order
            log.LogInformation($"Processing payment for order {order.OrderId}");
            // ...
        }
    }
}
