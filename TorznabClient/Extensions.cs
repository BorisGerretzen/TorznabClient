using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TorznabClient;

public static class Extensions
{
    /// <summary>
    ///     Adds a Torznab client to the service collection.
    /// </summary>
    /// <param name="services">DI container.</param>
    /// <param name="configuration">Configuration.</param>
    /// <param name="sectionName">Name of the torznab configuration section, <see cref="TorznabClientOptions" />.</param>
    /// <param name="configureClient">Callback to configure the HTTP client.</param>
    /// <param name="configureClientBuilder">Callback to configure the Http client builder, e.g. to add Polly.</param>
    public static IServiceCollection AddTorznabClient(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = TorznabClientOptions.SectionName,
        Action<IServiceProvider, HttpClient>? configureClient = null,
        Action<IHttpClientBuilder>? configureClientBuilder = null)
    {
        services.Configure<TorznabClientOptions>(configuration.GetSection(sectionName));

        configureClient ??= (provider, client) =>
        {
            var baseUrl = provider.GetRequiredService<IOptions<TorznabClientOptions>>().Value.Url;
            client.BaseAddress = new Uri(baseUrl ?? throw new InvalidOperationException("Torznab URL is not configured."));
        };
        var builder = services.AddHttpClient<ITorznabClient, TorznabClient>(configureClient);
        configureClientBuilder?.Invoke(builder);

        return services;
    }
}