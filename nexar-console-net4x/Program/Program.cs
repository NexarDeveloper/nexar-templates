using Microsoft.Extensions.DependencyInjection;
using Nexar.Client;
using StrawberryShake;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // assume the Nexar token is set as the environment variable
        var nexarToken = Environment.GetEnvironmentVariable("NEXAR_TOKEN") ?? throw new InvalidOperationException("Please set environment 'NEXAR_TOKEN'");

        // create and configure the Nexar client
        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddNexarClient()
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://api.nexar.com/graphql");
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {nexarToken}");
            });
        var services = serviceCollection.BuildServiceProvider();
        var nexarClient = services.GetRequiredService<NexarClient>();

        // loop of searches
        for (; ; )
        {
            // prompt for an MPN to search
            Console.Write("Search MPN: ");
            var mpn = Console.ReadLine();
            if (string.IsNullOrEmpty(mpn))
                return;

            // invoke the generated query with the parameter and check for errors
            var result = await nexarClient.SearchMpn.ExecuteAsync(mpn);
            result.EnsureNoErrors();
            if (result.Data.SupSearchMpn.Results == null)
                continue;

            // process (print) the strongly typed results
            foreach (var it in result.Data.SupSearchMpn.Results)
            {
                Console.WriteLine($"MPN: {it.Part.Mpn}");
                Console.WriteLine($"Desciption: {it.Part.ShortDescription}");
                Console.WriteLine($"Manufacturer: {it.Part.Manufacturer.Name}");
                Console.WriteLine();
            }
        }
    }
}
