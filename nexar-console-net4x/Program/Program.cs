using Nexar.Client;
using StrawberryShake;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // get the Nexar token
        var nexarToken = Environment.GetEnvironmentVariable("NEXAR_TOKEN") ?? throw new InvalidOperationException("Please set environment 'NEXAR_TOKEN'");

        // create the Nexar client
        var nexarClient = NexarClientFactory.CreateClient(httpClient =>
        {
            httpClient.BaseAddress = new Uri("https://api.nexar.com/graphql");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {nexarToken}");
        });

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
