using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TorznabClient.Jackett;

namespace TorznabClient.Demo;

public class ExampleService(IJackettClient client, ILogger<ExampleService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var indexers = await GetIndexers();

        await foreach (var result in client.SearchAsync(query: "doctor who", indexers: indexers).WithCancellation(stoppingToken))
            logger.LogInformation("Got {numResults} results from {indexer}", result.Channel?.Releases.Count, result.Channel?.Title);

        Environment.Exit(0);
    }

    /// <summary>
    ///     Gets the configured indexer IDs from Jackett.
    /// </summary>
    private async Task<IEnumerable<string>> GetIndexers()
    {
        var indexers = await client.GetIndexersAsync(configured: true);
        return indexers
            .Select(indexer => indexer.Id!);
    }
}