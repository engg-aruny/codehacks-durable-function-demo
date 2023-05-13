using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace codehacks_durable_function_demo
{
    public static class UpdateInventoryFunction
    {
        [FunctionName("UpdateInventory")]
        public static void Run([ActivityTrigger] Order order, ILogger log)
        {
            // Update the inventory to reflect the order
            log.LogInformation($"Updating inventory for order {order.OrderId}");
            // ...
        }
    }
}
