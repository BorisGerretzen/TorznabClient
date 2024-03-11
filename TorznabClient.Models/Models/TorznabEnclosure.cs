namespace TorznabClient.Models.Models;

public record TorznabEnclosure
{
    private TorznabEnclosure()
    {
    }

    [XmlAttribute("url")] public string? Url { get; init; }

    [XmlAttribute("length")] public long Length { get; init; }

    [XmlAttribute("type")] public string? Type { get; init; }
}