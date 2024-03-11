namespace TorznabClient.Models.Models;

public record TorznabChannel
{
    private TorznabChannel()
    {
        Releases = [];
    }

    [XmlElement("link", Namespace = Constants.AtomNamespace)]
    public TorznabAtomLink? AtomLink { get; init; }

    [XmlElement("title")] public string? Title { get; init; }

    [XmlElement("description")] public string? Description { get; init; }

    [XmlElement("link")] public string? Link { get; init; }

    [XmlElement("language")] public string? Language { get; init; }

    [XmlElement("category")] public string? Category { get; init; }

    [XmlElement("item")] public List<TorznabRelease> Releases { get; init; }
}