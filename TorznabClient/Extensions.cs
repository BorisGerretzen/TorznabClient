using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TorznabClient.Exceptions;
using TorznabClient.Jackett;

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
            client.BaseAddress = new Uri(baseUrl ?? throw new TorznabConfigurationException("Torznab URL is not configured."));
        };
        var builder = services.AddHttpClient<ITorznabClient, TorznabClient>(configureClient);
        configureClientBuilder?.Invoke(builder);

        return services;
    }

    /// <summary>
    ///     Adds a Jackett client to the service collection.
    ///     Internally, this method adds a <see cref="ITorznabClient" /> and a <see cref="IJackettClient" /> to the service collection.
    /// </summary>
    /// <param name="services">DI container.</param>
    /// <param name="configuration">Configuration.</param>
    /// <param name="sectionName">Name of the torznab configuration section, <see cref="JackettClientOptions" />.</param>
    /// <param name="configureClient">Callback to configure the HTTP client.</param>
    /// <param name="configureClientBuilder">Callback to configure the Http client builder, e.g. to add Polly.</param>
    /// <remarks>
    /// <see cref="configureClient"/> and <see cref="configureClientBuilder"/> will be called twice, once for the Torznab client and once for the Jackett client.
    /// </remarks>
    public static IServiceCollection AddJackettClient(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = JackettClientOptions.SectionName,
        Action<IServiceProvider, HttpClient>? configureClient = null,
        Action<IHttpClientBuilder>? configureClientBuilder = null)
    {
        services.Configure<JackettClientOptions>(configuration.GetSection(sectionName));
        services.Configure<TorznabClientOptions>(configuration.GetSection(sectionName));

        configureClient ??= (provider, client) =>
        {
            var baseUrl = provider.GetRequiredService<IOptions<JackettClientOptions>>().Value.Url;
            client.BaseAddress = new Uri(baseUrl ?? throw new TorznabConfigurationException("Torznab URL is not configured."));
        };

        var builder = services.AddHttpClient<IJackettClient, JackettClient>(configureClient);
        configureClientBuilder?.Invoke(builder);

        builder = services.AddHttpClient<ITorznabClient, TorznabClient>(configureClient);
        configureClientBuilder?.Invoke(builder);

        return services;
    }
}