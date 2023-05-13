
### About Durable Functions

Azure Durable Functions is a serverless extension of Azure Functions that allows developers to build stateful workflows and orchestrations in a simple and scalable way. The main advantage of Durable Functions is that it enables developers to write long-running, stateful workflows in a serverless environment, without having to worry about managing the underlying infrastructure.

In this article, we'll be focusing on C#, because now, Durable Functions do need to be written in a .NET language. Also we will see code to coordinate a workflow that involves multiple microservices, such as processing a customer order, handling payments, and updating inventory.

### What is Serverless?

The concept of serverless computing involves the ability to deploy code without concerning ourselves with the underlying infrastructure. When we deploy Azure Functions, there is no need to specify the number of servers required to execute the functions. The Azure Functions Runtime will automatically allocate sufficient resources to handle the required workload.

### Azure Function Pricing Model

Azure Functions has a flexible pricing model that allows developers to pay only for the resources that they use. There are two pricing tiers available: a Consumption plan and an App Service plan.

**The Consumption plan** is a pay-as-you-go model that charges users based on the number of executions and the execution time of their functions. This model is ideal for applications with unpredictable workloads, as it automatically scales up or down based on demand. There is no upfront cost, and users are only charged for the resources consumed by their functions. The cost of each function execution is determined by the amount of memory used, the duration of the execution, and the number of times the function is triggered.

**The App Service plan** is a fixed-price model that provides a dedicated set of resources for running Azure Functions. Users can choose the size and capacity of their plan, and are charged a fixed rate based on the plan that they select. This model is ideal for applications with predictable workloads, as users can choose the resources they need in advance, and can scale up or down as needed. This model also provides more control over the infrastructure and allows for more fine-tuning of performance.

### Use Cases for Durable Functions

Durable Functions are especially useful in the following scenarios:

1. **Long-running workflows:** Durable Functions make it easy to define and execute workflows that involve multiple steps and long-running processes. For example, a workflow might involve processing a large amount of data, invoking multiple functions to transform the data, and then storing the results in a database.

2. **Event-driven architectures:** Durable Functions can be used to orchestrate event-driven architectures, where multiple functions are triggered by events and need to communicate with each other to complete a task. For example, a customer order might trigger a series of functions that handle order processing, inventory management, and shipping.

3. **Microservices communication:** Durable Functions can be used to coordinate communication between microservices, allowing them to work together to complete a larger task. For example, a function might be used to coordinate a workflow that involves multiple microservices, such as processing a customer order, handling payments, and updating inventory.

### Why Durable Functions
- Easy state management, 
- Event-driven architecture support, 
- Built-in retry and error handling, 
- Scalability and performance, 
- Integration with Azure services


### Best Practices for Durable Functions

It's important to follow certain best practices to ensure their durability, scalability, and maintainability.

**One of the most important** best practices for Durable Functions is to use the [Fan-Out/Fan-In](https://learn.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-overview?tabs=csharp-inproc#fan-in-out) pattern to break down long-running workflows into smaller, more manageable tasks. This pattern involves creating a set of parallel tasks that can be executed simultaneously, and then collecting the results of those tasks and using them to make a final decision or to trigger additional tasks.

**Another best practice** is to use the Durable Task Framework's **built-in checkpointing mechanism** to persist the state of the workflow to durable storage. This ensures that if the workflow is interrupted or if the underlying infrastructure fails, it can be resumed from the last checkpoint instead of starting over from the beginning.

It's also important to carefully design the input and output contracts for each activity function, and to ensure that they are idempotent and fault-tolerant. This means that if an activity function fails, it can be retried without causing unintended side effects or duplicating work.

Finally, it's important to monitor the execution of Durable Functions workflows using Azure Application Insights or other monitoring tools, and to use the insights gained from monitoring to optimize the performance and cost-effectiveness of the workflows.

### Creating a Function App project for Durable Function

Real-life example of a durable function is an e-commerce website that processes orders. When a customer places an order, the website will start a durable function to handle the processing of that order.

The durable function will first **validate the order** and **check that all the required information is present**. Then, it will use external services to calculate taxes, shipping costs, and any discounts that apply to the order.

Next, the durable function will create a record of the order in the **website's database** and **send a notification** to the customer that their order has been received.

Finally, the durable function will trigger additional functions to handle tasks like **inventory management, payment processing, and shipping fulfillment**. Throughout this process, the durable function will persist its state and allow for graceful handling of errors and retries, ensuring that the order is processed successfully and efficiently.

![Real-life example](https://www.dropbox.com/s/a6dwbpyyfcigint/Cover_image_Durablefunctions_v1.jpg?raw=1 "Real-life example")

#### Implementation

```bash
Install-Package Microsoft.Azure.WebJobs.Extensions.DurableTask
```

**ProcessOrderFunction**
```csharp
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

```
> The full source code for this article can be found on [GitHub](https://github.com/engg-aruny/codehacks-durable-function-demo).

![Running Example](https://www.dropbox.com/s/an53gurr4tyb1vp/running_sample_app.jpg?raw=1 "Running Example")


### Limitations and Alternatives

While Durable Functions provide many benefits, there are some limitations to consider when deciding whether they are the right choice for your application. Additionally, there are alternatives to Durable Functions that may better fit your use case. Here are some key considerations:

#### Limitations:
**Vendor lock-in:** Durable Functions are tightly integrated with Azure, which means that if you use them, you are locked into the Azure platform. This may be a limitation if you need to migrate your application to another cloud provider in the future.

**Complexity:** While Durable Functions provide a powerful framework for managing complex workflows, they can also introduce additional complexity to your code. If you are only dealing with simple workflows, it may be overkill to use Durable Functions.

**Cost:** Durable Functions can incur additional costs compared to traditional serverless functions, especially if your workflows require large state storage.

#### Alternatives:
**Step Functions:** AWS Step Functions is an alternative to Durable Functions, providing a similar workflow orchestration framework in the AWS ecosystem. Step Functions provide similar benefits to Durable Functions, including easy state management, event-driven architecture support, and built-in error handling.

**Logic Apps:** Azure Logic Apps provide another way to orchestrate workflows in Azure, using a visual designer to create workflows with pre-built connectors to various Azure services and third-party services. Logic Apps provide similar benefits to Durable Functions, including easy integration with other Azure services.

**Custom code:** In some cases, it may be simpler and more cost-effective to write custom code to handle your workflow needs. However, custom code can be more difficult to maintain and scale as your application grows.

### Summary
Overall, Durable Functions provide a powerful and flexible way to build complex, long-running workflows in Azure Functions. With easy state management, event-driven architecture support, built-in retry and error handling, scalability and performance, and integration with Azure services, they are a great choice for many types of applications.
