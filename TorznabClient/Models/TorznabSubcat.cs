namespace TorznabClient.Models;

public record TorznabSubcat
{
    private TorznabSubcat()
    {
    }

    [XmlAttribute("id")]
    public int Id { get; init; }

    [XmlAttribute("name")]
    public string? Name { get; init; }
}