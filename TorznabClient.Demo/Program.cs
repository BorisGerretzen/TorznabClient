using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TorznabClient;
using TorznabClient.Demo;

// This is a simple example of how to use the TorznabClient to search for torrents using Jackett.
// To run this example, make sure you have a running Jackett instance and have the API key and URL available.
// Add your Jackett API key and URL to the appsettings.json file, then run the application.
// The application will search for torrents matching the query "doctor who" and log the results.

await Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<ExampleService>();
        services.AddJackettClient(context.Configuration);
    })
    .RunConsoleAsync();