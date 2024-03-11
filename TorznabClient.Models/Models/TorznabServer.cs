namespace TorznabClient.Models.Models;

public record TorznabServer
{
    private TorznabServer()
    {
    }

    [XmlAttribute("version")] public string? Version { get; init; }
    [XmlAttribute("title")] public string? Title { get; init; }
    [XmlAttribute("strapline")] public string? Strapline { get; init; }
    [XmlAttribute("email")] public string? Email { get; init; }
    [XmlAttribute("url")] public string? Url { get; init; }
    [XmlAttribute("image")] public string? Image { get; init; }
}