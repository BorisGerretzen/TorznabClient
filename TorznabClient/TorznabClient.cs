using System.Web;
using Microsoft.Extensions.Options;
using TorznabClient.Models;
using TorznabClient.Models.Exceptions;
using TorznabClient.Models.Models;

namespace TorznabClient;

public class TorznabClient(IOptions<TorznabClientOptions> options, HttpClient client) : ITorznabClient
{
    private readonly string _apiKey = options.Value.ApiKey ?? throw new InvalidOperationException("API key is not configured.");

    public Task<TorznabCaps> GetCapsAsync(string? apiKey = null)
    {
        apiKey ??= _apiKey;
        var parameters = new Dictionary<string, object?>
        {
            ["t"] = "caps",
            ["apikey"] = apiKey
        };
        return DoRequestAsync<TorznabCaps>(parameters);
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
        string? sort = null)
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
        return DoRequestAsync<TorznabRss>(parameters);
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
        int? offset = null)
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
        return DoRequestAsync<TorznabRss>(parameters);
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
        int? offset = null)
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
        return DoRequestAsync<TorznabRss>(parameters);
    }

    /// <summary>
    ///     Does a request to the Torznab server and deserializes the response.
    /// </summary>
    /// <param name="parameters">Parameters to build the querystring from.</param>
    /// <typeparam name="TResponse">Response type.</typeparam>
    /// <returns>Deserialized object.</returns>
    /// <exception cref="TorznabException">If deserialization failed.</exception>
    private async Task<TResponse> DoRequestAsync<TResponse>(Dictionary<string, object?> parameters)
    {
        var queryString = BuildQueryString(parameters);
        var response = await client.GetAsync(queryString);
        response.EnsureSuccessStatusCode();
        var stream = await response.Content.ReadAsStreamAsync();

        var serializer = new TorznabSerializer<TResponse>();
        var result = serializer.Deserialize(stream);

        return result ?? throw new TorznabException(-1, "Failed to deserialize Torznab response.");
    }

    /// <summary>
    ///     Builds a query string from a dictionary of parameters.
    /// </summary>
    /// <param name="parameters">Parameters, null values will be skipped.</param>
    /// <returns>Query string.</returns>
    private static string BuildQueryString(Dictionary<string, object?> parameters)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);

        foreach (var parameter in parameters)
        {
            if (parameter.Value is null)
                continue;

            var valueString = parameter.Value switch
            {
                IEnumerable<string> enumerable => string.Join(",", enumerable),
                IEnumerable<int> enumerable => string.Join(",", enumerable),
                string or int or bool or long => parameter.Value.ToString(),
                _ => throw new NotSupportedException($"Unknown type {parameter.Value.GetType()}.")
            };

            if (string.IsNullOrEmpty(valueString))
                continue;

            query[parameter.Key] = valueString;
        }

        return "?" + query;
    }
}