namespace TorznabClient.Models;

public record TorznabIndexer
{
    private TorznabIndexer()
    {
    }

    [XmlAttribute("id")]
    public string? Id { get; init; }

    [XmlAttribute("configured")]
    public bool Configured { get; init; }

    [XmlElement("title")]
    public string? Title { get; init; }

    [XmlElement("description")]
    public string? Description { get; init; }

    [XmlElement("link")]
    public string? Link { get; init; }

    [XmlElement("language")]
    public string? Language { get; init; }

    [XmlElement("type")]
    public string? Type { get; init; }

    [XmlElement("caps")]
    public TorznabCaps? Caps { get; init; }
}