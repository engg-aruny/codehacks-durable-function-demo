using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;

namespace codehacks_durable_function_demo.Calculates
{
    public static class CalculateTaxesFunction
    {
        [FunctionName("CalculateTaxes")]
        public static decimal Run([ActivityTrigger] Order order)
        {
            // Calculate taxes based on the order amount and tax rate
            var taxes = order.TotalPrice * order.TaxRate;
            return taxes;
        }
    }
}
