using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.Threading.Tasks;

namespace codehacks_durable_function_demo
{
    public static class ProcessOrderFunction
    {
        [FunctionName("ProcessOrder")]
        public static async Task Run(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            // Retrieve the order from the input
            var order = context.GetInput<Order>();

            // Validate the order
            if (!order.IsValid())
            {
                // If the order is invalid, log an error and exit
                context.SetOutput("Order is invalid");
                return;
            }

            // Calculate taxes, shipping costs, and discounts
            var taxes = await context.CallActivityAsync<decimal>("CalculateTaxes", order);
            var shipping = await context.CallActivityAsync<decimal>("CalculateShipping", order);
            var discount = await context.CallActivityAsync<decimal>("CalculateDiscount", order);

            // Create a record of the order in the database
            await context.CallActivityAsync("CreateOrderRecord", order);

            // Send a notification to the customer
            await context.CallActivityAsync("SendOrderNotification", order);

            // Trigger additional functions to handle inventory management, payment processing, and shipping fulfillment
            var inventoryTask = context.CallActivityAsync("UpdateInventory", order);
            var paymentTask = context.CallActivityAsync("ProcessPayment", order);
            var shippingTask = context.CallActivityAsync("FulfillShipping", order);

            // Wait for all the functions to complete
            await Task.WhenAll(inventoryTask, paymentTask, shippingTask);

            // Exit successfully
            context.SetOutput("Order processed successfully");
        }
    }
}
