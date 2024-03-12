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
        {
            if (result.IsError)
            {
                logger.LogError(result.Error, "Failed to search");
                continue;
            }

            logger.LogInformation("Found {numResults} results from indexer {indexer}", result.Result!.Channel?.Releases.Count, result.Result!.Channel?.Title);
        }

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