# TorznabClient
This is a C# client for the [Torznab](https://torznab.github.io/spec-1.3-draft/torznab/) protocol, it also works with [Jackett](https://github.com/Jackett/Jackett).

# Usage
## Directly
You can use the `TorznabClient` class directly, but you will need to manage the `HttpClient` yourself.
```csharp
using Microsoft.Extensions.Options;
using TorznabClient;

var options = Options.Create(new TorznabClientOptions
{
    Url = "http://localhost:9117/api/v2.0/indexers/all/results/torznab",
    ApiKey = "your_api_key"
});

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri(options.Value.Url!);
var client = new TorznabClient.TorznabClient(options, httpClient);
var caps = await client.GetCapsAsync();

Console.WriteLine(caps);
```

## Using Dependency Injection
You can use the `AddTorznabClient` extension method to add the `ITorznabClient` to the service collection.
The configuration is expected to be in the `TorznabClient` section of the configuration, but you can change that by passing a `sectionName` to the `AddTorznabClient` method.

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