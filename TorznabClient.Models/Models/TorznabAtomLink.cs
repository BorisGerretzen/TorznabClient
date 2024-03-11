namespace TorznabClient.Models.Models;

public record TorznabAtomLink
{
    private TorznabAtomLink()
    {
    }

    [XmlAttribute("href")] public string? Href { get; init; }

    [XmlAttribute("rel")] public string? Rel { get; init; }

    [XmlAttribute("type")] public string? Type { get; init; }
}