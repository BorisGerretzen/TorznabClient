using Microsoft.Extensions.Options;
using TorznabClient.Exceptions;
using TorznabClient.Models;

namespace TorznabClient.Torznab;

public class TorznabClient(IOptions<TorznabClientOptions> options, HttpClient client) : BaseClient(client), ITorznabClient
{
    private readonly string _apiKey = options.Value.ApiKey ?? throw new TorznabConfigurationException("API key is not configured.");

    public Task<TorznabCaps> GetCapsAsync(
        string? apiKey = null,
        string? url = null)
    {
        apiKey ??= _apiKey;
        var parameters = new Dictionary<string, object?>
        {
            ["t"] = "caps",
            ["apikey"] = apiKey
        };
        return DoRequestAsync<TorznabCaps>(parameters, url);
    }

    public Task<TorznabRss> SearchAsync(
        string? apiKey = null,
        string? query = null,
        IEnumerable<string>? groups = null,
        int? limit = null,
        IEnumerable<int>? categories = null,
        IEnumerable<string>? attributes = null,
        bool? extended = null,
        bool? delete = null,
        int? maxAge = null,
        long? minSize = null,
        long? maxSize = null,
        int? offset = null,
        string? sort = null,
        string? url = null)
    {
        apiKey ??= _apiKey;
        var parameters = new Dictionary<string, object?>
        {
            ["t"] = "search",
            ["apikey"] = apiKey,
            ["q"] = query,
            ["group"] = groups,
            ["limit"] = limit,
            ["cat"] = categories,
            ["attrs"] = attributes,
            ["extended"] = extended,
            ["del"] = delete,
            ["maxage"] = maxAge,
            ["minsize"] = minSize,
            ["maxsize"] = maxSize,
            ["offset"] = offset,
            ["sort"] = sort
        };
        return DoRequestAsync<TorznabRss>(parameters, url);
    }

    public Task<TorznabRss> TvSearchAsync(
        string? apiKey = null,
        string? query = null,
        string? season = null,
        string? episode = null,
        int? limit = null,
        string? tvRageId = null,
        int? tvMazeId = null,
        int? tvDbId = null,
        IEnumerable<int>? categories = null,
        IEnumerable<string>? attributes = null,
        bool? extended = null,
        bool? delete = null,
        int? maxAge = null,
        int? offset = null,
        string? url = null)
    {
        apiKey ??= _apiKey;

        var parameters = new Dictionary<string, object?>
        {
            ["t"] = "tvsearch",
            ["apikey"] = apiKey,
            ["q"] = query,
            ["season"] = season,
            ["ep"] = episode,
            ["limit"] = limit,
            ["rid"] = tvRageId,
            ["tvmazeid"] = tvMazeId,
            ["tvdbid"] = tvDbId,
            ["cat"] = categories,
            ["attrs"] = attributes,
            ["extended"] = extended,
            ["del"] = delete,
            ["maxage"] = maxAge,
            ["offset"] = offset
        };
        return DoRequestAsync<TorznabRss>(parameters, url);
    }

    public Task<TorznabRss> MovieSearchAsync(
        string? apiKey = null,
        string? query = null,
        string? imdbId = null,
        IEnumerable<int>? categories = null,
        string? genre = null,
        IEnumerable<string>? attributes = null,
        bool? extended = null,
        bool? delete = null,
        int? maxAge = null,
        int? offset = null,
        string? url = null)
    {
        apiKey ??= _apiKey;

        var parameters = new Dictionary<string, object?>
        {
            ["t"] = "movie",
            ["apikey"] = apiKey,
            ["q"] = query,
            ["imdbid"] = imdbId,
            ["cat"] = categories,
            ["genre"] = genre,
            ["attrs"] = attributes,
            ["extended"] = extended,
            ["del"] = delete,
            ["maxage"] = maxAge,
            ["offset"] = offset
        };
        return DoRequestAsync<TorznabRss>(parameters, url);
    }
}