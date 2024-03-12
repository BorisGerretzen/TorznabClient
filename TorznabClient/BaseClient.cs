using System.Web;
using TorznabClient.Exceptions;
using TorznabClient.Serializer;

namespace TorznabClient;

public abstract class BaseClient(HttpClient client)
{
    /// <summary>
    ///     Does a request to the Torznab server and deserializes the response.
    /// </summary>
    /// <param name="parameters">Parameters to build the querystring from.</param>
    /// <param name="url">Overrides configured url.</param>
    /// <typeparam name="TResponse">Response type.</typeparam>
    /// <returns>Deserialized object.</returns>
    /// <exception cref="TorznabException">If deserialization failed.</exception>
    protected async Task<TResponse> DoRequestAsync<TResponse>(Dictionary<string, object?> parameters, string? url = null)
    {
        var queryString = BuildQueryString(parameters);
        HttpResponseMessage response;
        if (!string.IsNullOrEmpty(url))
            response = await client.GetAsync(new Uri(new Uri(url), queryString));
        else
            response = await client.GetAsync(queryString);
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
    protected static string BuildQueryString(Dictionary<string, object?> parameters)
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