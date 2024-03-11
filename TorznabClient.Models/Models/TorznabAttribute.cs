namespace TorznabClient.Models.Models;

public record TorznabAttribute
{
    private TorznabAttribute()
    {
    }

    [XmlAttribute("name")] public string? Name { get; init; }

    [XmlAttribute("value")] public string? Value { get; init; }
}