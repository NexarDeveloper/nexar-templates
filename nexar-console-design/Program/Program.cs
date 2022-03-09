using Microsoft.Extensions.DependencyInjection;
using Nexar.Client;
using Nexar.Client.Login;
using StrawberryShake;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // assume Nexar client ID and secret are set as environment variables
        var clientId = Environment.GetEnvironmentVariable("NEXAR_CLIENT_ID") ?? throw new InvalidOperationException("Please set environment 'NEXAR_CLIENT_ID'");
        var clientSecret = Environment.GetEnvironmentVariable("NEXAR_CLIENT_SECRET") ?? throw new InvalidOperationException("Please set environment 'NEXAR_CLIENT_SECRET'");

        // sign in and get the token
        var login = await LoginHelper.LoginAsync(clientId, clientSecret, new string[] { "user.access", "design.domain" });
        var username = login.Username;
        var nexarToken = login.AccessToken;

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

        // invoke the generated query and check for errors
        var result = await nexarClient.Workspaces.ExecuteAsync();
        result.EnsureNoErrors();

        // process (print) the strongly typed results
        Console.WriteLine($"{username} workspaces:");
        foreach (var workspace in result.Data.DesWorkspaces)
        {
            Console.WriteLine();
            Console.WriteLine($"id: {workspace.Id}");
            Console.WriteLine($"url: {workspace.Url}");
            Console.WriteLine($"name: {workspace.Name}");
        }
    }
}
