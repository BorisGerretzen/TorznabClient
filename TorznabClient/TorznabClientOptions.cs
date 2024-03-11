using System.ComponentModel.DataAnnotations;

namespace TorznabClient;

public class TorznabClientOptions
{
    public const string SectionName = "TorznabClient";

    public string? Url { get; init; }

    [Required] public string? ApiKey { get; init; }
}