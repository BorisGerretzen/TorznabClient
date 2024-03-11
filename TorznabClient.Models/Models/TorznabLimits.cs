namespace TorznabClient.Models.Models;

public record TorznabLimits
{
    private TorznabLimits()
    {
    }

    [XmlAttribute("default")] public int Default { get; init; }

    [XmlAttribute("max")] public int Max { get; init; }
}