using Microsoft.Extensions.DependencyInjection;
using Nexar.Client;
using Nexar.Client.Token;
using StrawberryShake;
using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromDays(1);

    static async Task Main()
    {
        // assume Nexar client ID and secret are set as environment variables
        var clientId = Environment.GetEnvironmentVariable("NEXAR_CLIENT_ID") ?? throw new InvalidOperationException("Please set environment 'NEXAR_CLIENT_ID'");
        var clientSecret = Environment.GetEnvironmentVariable("NEXAR_CLIENT_SECRET") ?? throw new InvalidOperationException("Please set environment 'NEXAR_CLIENT_SECRET'");

        // get the token
        var httpClient = new HttpClient();
        var tokenExpiresAt = DateTime.UtcNow + TokenLifetime;
        var token = await httpClient.GetNexarTokenAsync(clientId, clientSecret);

        // create and configure the Nexar client
        var nexarClient = GetNexarClient(token);

        // loop of searches
        for (; ; )
        {
            // prompt for an MPN to search
            Console.Write("Search MPN: ");
            var mpn = Console.ReadLine();
            if (string.IsNullOrEmpty(mpn))
                return;

            if (DateTime.UtcNow >= tokenExpiresAt)
            {
                // token has expired, request a new one
                tokenExpiresAt = DateTime.UtcNow + TokenLifetime;
                token = await httpClient.GetNexarTokenAsync(clientId, clientSecret);
                nexarClient = GetNexarClient(token);
            }

            // invoke the generated query with the parameter and check for errors
            var result = await nexarClient.SearchMpn.ExecuteAsync(mpn);
            result.EnsureNoErrors();

            if (result.Data.SupSearchMpn.Results == null)
            {
                continue;
            }

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

    private static NexarClient GetNexarClient(string token)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddNexarClient()
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://api.nexar.com/graphql");
                httpClient.DefaultRequestHeaders.Add("token", token);
            });
        var services = serviceCollection.BuildServiceProvider();
        return services.GetRequiredService<NexarClient>();
    }
}
