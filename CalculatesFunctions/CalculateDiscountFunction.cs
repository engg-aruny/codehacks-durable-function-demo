using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;

namespace codehacks_durable_function_demo.Calculates
{
    public static class CalculateDiscountFunction
    {
        [FunctionName("CalculateDiscount")]
        public static decimal Run([ActivityTrigger] Order order)
        {
            // Calculate any discounts that apply to the order
            var discount = 0m;
            
            // Apply a 10% discount for orders over $500
            if (order.TotalPrice >= 500m)
            {
                discount = order.TotalPrice * 0.1m;
            }
            return discount;

        }
    }
}
