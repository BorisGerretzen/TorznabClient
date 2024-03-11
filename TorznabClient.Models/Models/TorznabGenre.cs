namespace TorznabClient.Models.Models;

public record TorznabGenre
{
    private TorznabGenre()
    {
    }

    [XmlAttribute("id")] public int Id { get; init; }
    [XmlAttribute("categoryid")] public int CategoryId { get; init; }
    [XmlAttribute("name")] public string? Name { get; init; }
}