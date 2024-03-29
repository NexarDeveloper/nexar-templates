﻿using Nexar.Client;
using Nexar.Client.Token;
using StrawberryShake;
using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(12);
    private static DateTime NexarTokenExpiresAt;
    private static string NexarToken;

    /// <summary>
    /// Updates the cached Nexar access token.
    /// </summary>
    private static async Task UpdateNexarTokenAsync()
    {
        // use the existing not expired token
        if (NexarToken != null && DateTime.UtcNow < NexarTokenExpiresAt)
            return;

        // get the Nexar client ID and secret
        var clientId = Environment.GetEnvironmentVariable("NEXAR_CLIENT_ID") ?? throw new InvalidOperationException("Please set environment 'NEXAR_CLIENT_ID'");
        var clientSecret = Environment.GetEnvironmentVariable("NEXAR_CLIENT_SECRET") ?? throw new InvalidOperationException("Please set environment 'NEXAR_CLIENT_SECRET'");

        // get and update the Nexar token
        using var httpClient = new HttpClient();
        NexarTokenExpiresAt = DateTime.UtcNow + TokenLifetime;
        NexarToken = await httpClient.GetNexarTokenAsync(clientId, clientSecret);
    }

    static async Task Main()
    {
        // create the Nexar client
        var nexarClient = NexarClientFactory.CreateClient(httpClient =>
        {
            httpClient.BaseAddress = new Uri("https://api.nexar.com/graphql");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {NexarToken}");
        });

        // loop of searches
        for (; ; )
        {
            // prompt for an MPN to search
            Console.Write("Search MPN: ");
            var mpn = Console.ReadLine();
            if (string.IsNullOrEmpty(mpn))
                return;

            // update the token
            await UpdateNexarTokenAsync();

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
