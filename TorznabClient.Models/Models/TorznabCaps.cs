namespace TorznabClient.Models.Models;

[XmlRoot("caps")]
public record TorznabCaps
{
    private TorznabCaps()
    {
        Categories = [];
        Groups = [];
        Genres = [];
        Tags = [];
    }

    [XmlElement("server")] public TorznabServer? Server { get; init; }

    [XmlElement("limits")] public TorznabLimits? Limits { get; init; }

    [XmlElement("retention")] public TorznabRetention? Retention { get; init; }

    [XmlElement("registration")] public TorznabRegistration? Registration { get; init; }

    [XmlElement("searching")] public TorznabSearching? Searching { get; init; }

    [XmlArray("categories")]
    [XmlArrayItem("category")]
    public List<TorznabCategory> Categories { get; init; }

    [XmlArray("groups")]
    [XmlArrayItem("group")]
    public List<TorznabGroup> Groups { get; init; }

    [XmlArray("genres")]
    [XmlArrayItem("genre")]
    public List<TorznabGenre> Genres { get; init; }

    [XmlArray("tags")]
    [XmlArrayItem("tag")]
    public List<TorznabTag> Tags { get; init; }
}