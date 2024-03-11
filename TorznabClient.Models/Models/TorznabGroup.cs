namespace TorznabClient.Models.Models;

public record TorznabGroup
{
    [XmlAttribute("id")] public int Id { get; init; }

    [XmlAttribute("name")] public string? Name { get; init; }

    [XmlAttribute("description")] public string? Description { get; init; }

    [XmlAttribute("lastupdate")] public string? LastUpdate { get; init; }
}