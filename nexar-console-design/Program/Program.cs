using Nexar.Client;
using Nexar.Client.Login;
using StrawberryShake;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // get the Nexar client ID and secret
        var clientId = Environment.GetEnvironmentVariable("NEXAR_CLIENT_ID") ?? throw new InvalidOperationException("Please set environment 'NEXAR_CLIENT_ID'");
        var clientSecret = Environment.GetEnvironmentVariable("NEXAR_CLIENT_SECRET") ?? throw new InvalidOperationException("Please set environment 'NEXAR_CLIENT_SECRET'");

        // login and get the Nexar token
        var login = await LoginHelper.LoginAsync(clientId, clientSecret, new string[] { "user.access", "design.domain" });
        var username = login.Username;
        var nexarToken = login.AccessToken;

        // create the Nexar client
        var nexarClient = NexarClientFactory.CreateClient(httpClient =>
        {
            httpClient.BaseAddress = new Uri("https://api.nexar.com/graphql");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {nexarToken}");
        });

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
