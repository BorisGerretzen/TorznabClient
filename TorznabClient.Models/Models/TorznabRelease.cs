namespace TorznabClient.Models.Models;

public record TorznabRelease
{
    private TorznabRelease()
    {
        Attributes = [];
        Categories = [];
    }

    [XmlElement("title")] public string? Title { get; init; }

    [XmlElement("guid")] public string? Guid { get; init; }

    [XmlElement("type")] public string? Type { get; init; }

    [XmlElement("comments")] public string? Comments { get; init; }

    [XmlElement("pubDate")] public string? PubDate { get; init; }

    [XmlElement("size")] public long? Size { get; init; }

    [XmlElement("grabs")] public int? Grabs { get; init; }

    [XmlElement("description")] public string? Description { get; init; }

    [XmlElement("enclosure")] public TorznabEnclosure? Enclosure { get; init; }

    [XmlElement("link")] public string? Link { get; init; }

    [XmlElement("category")] public List<int> Categories { get; init; }

    [XmlElement("attr", Namespace = Constants.TorznabNamespace)]
    public List<TorznabAttribute> Attributes { get; init; }
}