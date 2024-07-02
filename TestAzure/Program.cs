using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using System;
using System.Threading.Tasks;

namespace AzureResourceLister
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Use DefaultAzureCredential to pick up the managed identity when running in Azure
            var credential = new DefaultAzureCredential();

            // Create a new ArmClient instance
            var armClient = new ArmClient(credential);

            // Get the subscription ID of the current context (managed identity)
            var subscription = await armClient.GetDefaultSubscriptionAsync();
            if (subscription == null)
            {
                Console.WriteLine("No subscription found for the managed identity.");
                return;
            }

            Console.WriteLine($"Using subscription ID: {subscription.Id}");

            // List all resources in the subscription
            await foreach (var resource in subscription.GetGenericResourcesAsync())
            {
                var resourceData = resource.Data;

                Console.WriteLine($"Resource ID: {resourceData.Id}");
                Console.WriteLine($"Resource Name: {resourceData.Name}");
                Console.WriteLine($"Resource Type: {resourceData.ResourceType}");
                Console.WriteLine($"Resource Location: {resourceData.Location}");
                Console.WriteLine($"Resource Tags: {string.Join(", ", resourceData.Tags)}");

                if (resourceData.ManagedBy != null)
                {
                    Console.WriteLine($"Managed By: {resourceData.ManagedBy}");
                }

                if (resourceData.Kind != null)
                {
                    Console.WriteLine($"Kind: {resourceData.Kind}");
                }

                Console.WriteLine();
            }
        }
    }
}
