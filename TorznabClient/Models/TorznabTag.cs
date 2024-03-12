namespace TorznabClient.Models;

public record TorznabTag
{
    [XmlAttribute("name")]
    public string? Name { get; init; }

    [XmlAttribute("description")]
    public string? Description { get; init; }
}