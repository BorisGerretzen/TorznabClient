using Microsoft.Extensions.Options;
using TorznabClient.Exceptions;
using TorznabClient.Models.Models;

namespace TorznabClient.Jackett;

public class JackettClient(ITorznabClient torznabClient, HttpClient client, IOptions<JackettClientOptions> options) : BaseClient(client), IJackettClient
{
    private readonly string _apiKey = options.Value.ApiKey ?? throw new TorznabConfigurationException("API key is not configured.");
    private readonly string _url = options.Value.Url ?? throw new TorznabConfigurationException("Jackett URL is not configured.");

    public async Task<List<TorznabIndexer>> GetIndexersAsync(string? apiKey = null, bool? configured = null)
    {
        apiKey ??= _apiKey;
        var parameters = new Dictionary<string, object?>
        {
            ["t"] = "indexers",
            ["apikey"] = apiKey,
            ["configured"] = configured
        };
        var indexersResponse = await DoRequestAsync<TorznabIndexers>(parameters, GetUrl("all"));
        return indexersResponse.Indexers;
    }

    public IAsyncEnumerable<TorznabRss> SearchAsync(string? apiKey = null, string? query = null, IEnumerable<string>? groups = null, int? limit = null, IEnumerable<int>? categories = null, IEnumerable<string>? attributes = null,
        bool? extended = null, bool? delete = null,
        int? maxAge = null, long? minSize = null, long? maxSize = null, int? offset = null, string? sort = null, IEnumerable<string>? indexers = null)
    {
        indexers ??= ["all"];

        var tasks = indexers
            .Select(indexer => torznabClient.SearchAsync(apiKey, query, groups, limit, categories, attributes, extended, delete, maxAge, minSize, maxSize, offset, sort, GetUrl(indexer)));
        return ProcessTasks(tasks);
    }

    public IAsyncEnumerable<TorznabRss> TvSearchAsync(string? apiKey = null, string? query = null, string? season = null, string? episode = null, int? limit = null, string? tvRageId = null, int? tvMazeId = null, int? tvDbId = null,
        IEnumerable<int>? categories = null,
        IEnumerable<string>? attributes = null, bool? extended = null, bool? delete = null, int? maxAge = null, int? offset = null, IEnumerable<string>? indexers = null)
    {
        indexers ??= ["all"];

        var tasks = indexers
            .Select(indexer => torznabClient.TvSearchAsync(apiKey, query, season, episode, limit, tvRageId, tvMazeId, tvDbId, categories, attributes, extended, delete, maxAge, offset, GetUrl(indexer)));
        return ProcessTasks(tasks);
    }

    public IAsyncEnumerable<TorznabRss> MovieSearchAsync(string? apiKey = null, string? query = null, string? imdbId = null, IEnumerable<int>? categories = null, string? genre = null, IEnumerable<string>? attributes = null,
        bool? extended = null, bool? delete = null,
        int? maxAge = null, int? offset = null, IEnumerable<string>? indexers = null)
    {
        indexers ??= ["all"];

        var tasks = indexers
            .Select(indexer => torznabClient.MovieSearchAsync(apiKey, query, imdbId, categories, genre, attributes, extended, delete, maxAge, offset, GetUrl(indexer)));
        return ProcessTasks(tasks);
    }

    private static async IAsyncEnumerable<T> ProcessTasks<T>(IEnumerable<Task<T>> tasks)
    {
        var taskList = tasks.ToList();
        while (taskList.Count != 0)
        {
            var task = await Task.WhenAny(taskList);
            taskList.Remove(task);
            yield return task.Result;
        }
    }

    private string GetUrl(string indexer)
    {
        return new Uri(new Uri(_url), $"/api/v2.0/indexers/{indexer}/results/torznab").AbsoluteUri;
    }
}