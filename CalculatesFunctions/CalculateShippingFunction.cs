using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codehacks_durable_function_demo.Calculates
{
    public static class CalculateShippingFunction
    {
        [FunctionName("CalculateShipping")]
        public static decimal Run([ActivityTrigger] Order order)
        {
            // Calculate shipping costs based on the order weight and shipping rate
            var shipping = order.Weight * order.ShippingRate;
            return shipping;
        }
    }
}
