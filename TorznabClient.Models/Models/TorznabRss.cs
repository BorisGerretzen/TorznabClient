namespace TorznabClient.Models.Models;

[XmlRoot("rss")]
public record TorznabRss
{
    private TorznabRss()
    {
    }

    [XmlAttribute("version")] public string Version { get; init; } = "2.0";

    [XmlElement("channel")] public TorznabChannel? Channel { get; init; }
}