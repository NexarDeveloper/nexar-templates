using Microsoft.Extensions.DependencyInjection;
using StrawberryShake.Serialization;
using System;
using System.Net.Http;

namespace Nexar.Client
{
    public static class NexarClientFactory
    {
        /// <summary>
        /// Creates the Nexar GraphQL client.
        /// </summary>
        /// <param name="configureClient">
        /// Configures <see cref="HttpClient"/> for Nexar GraphQL requests.
        /// </param>
        /// <remarks>
        /// The specified client configuration action:
        /// <list>
        /// <item>
        /// Sets <see cref="HttpClient.BaseAddress"/> to the Nexar API endpoint,
        /// https://api.nexar.com/graphql or workspace location specific endpoint.
        /// </item>
        /// <item>
        /// Adds the "Authorization" header $"Bearer {nexarToken}"
        /// to <see cref="HttpClient.DefaultRequestHeaders"/>.
        /// </item>
        /// </list>
        /// </remarks>
        public static NexarClient CreateClient(Action<HttpClient> configureClient)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection
                // Add serializer for operations using Map.
                .AddSerializer(new JsonSerializer("Map"))
                // Add the Nexar GraphQL client.
                .AddNexarClient()
                // Configure each client request.
                .ConfigureHttpClient(configureClient);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider.GetRequiredService<NexarClient>();
        }
    }
}
