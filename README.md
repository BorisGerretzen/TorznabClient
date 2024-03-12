# TorznabClient
[![NuGet](https://img.shields.io/nuget/v/TorznabClient.svg)](https://www.nuget.org/packages/TorznabClient/)
[![Test](https://github.com/BorisGerretzen/TorznabClient/actions/workflows/test.yml/badge.svg)](https://github.com/BorisGerretzen/TorznabClient/actions/workflows/test.yml)


This is a C# client for the [Torznab](https://torznab.github.io/spec-1.3-draft/torznab/) protocol, it also works with [Jackett](https://github.com/Jackett/Jackett).

## Usage Jackett
You can use the `AddJackettClient` extension method to add the `IJackettClient` to the service collection.
The configuration is expected to be in the `JackettClient` section of the configuration, but you can change that by passing a `sectionName` to the `AddJackettClient` method.

Additionally you can take a look at the `TorznabClient.Demo` project for a complete example.

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TorznabClient;

var builder = Host.CreateApplicationBuilder();

// Load configuration, in this case from an in-memory collection.
var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
{
    ["JackettClient:Url"] = "http://localhost:9117/", // Note, only put the base url here.
    ["JackettClient:ApiKey"] = "your_api_key"
});
builder.Configuration.AddConfiguration(configurationBuilder.Build());

// Add Torznab client to the service collection.
builder.Services.AddJackettClient(builder.Configuration);

var host = builder.Build();

var client = host.Services.GetRequiredService<IJackettClient>();
var indexers = await client.GetIndexersAsync();

Console.WriteLine(indexers);
```

## Usage Torznab
You can use the `AddTorznabClient` extension method to add the `ITorznabClient` to the service collection.
The configuration is expected to be in the `TorznabClient` section of the configuration, but like in the previous section, you can change that by passing a `sectionName` to the `AddTorznabClient` method.

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TorznabClient;

var builder = Host.CreateApplicationBuilder();

// Load configuration, in this case from an in-memory collection.
var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
{
    ["TorznabClient:Url"] = "http://localhost:9117/api/v2.0/indexers/all/results/torznab",
    ["TorznabClient:ApiKey"] = "your_api_key"
});
builder.Configuration.AddConfiguration(configurationBuilder.Build());

// Add Torznab client to the service collection.
builder.Services.AddTorznabClient(builder.Configuration);

var host = builder.Build();

var client = host.Services.GetRequiredService<ITorznabClient>();
var caps = await client.GetCapsAsync();

Console.WriteLine(caps);
```

### Customizing the client
This works for both the Jackett as well as the Torznab client.
```csharp
// Specify a custom section name.
builder.Services.AddTorznabClient(builder.Configuration, sectionName: "CustomSectionName");

// Customize the HttpClient
builder.Services.AddTorznabClient(builder.Configuration, configureClient: (IServiceProvider provider, HttpClient httpClient) =>
{
    httpClient.Timeout = TimeSpan.FromSeconds(10);
    httpClient.DefaultRequestHeaders.Add("User-Agent", "TorznabClient.Demo");
});

// Use a Polly policy
builder.Services.AddTorznabClient(builder.Configuration, configureClientBuilder: (IHttpClientBuilder clientBuilder) =>
{
    clientBuilder.AddPolicyHandler(GetRetryPolicy());
});

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
            retryAttempt)));
}
```