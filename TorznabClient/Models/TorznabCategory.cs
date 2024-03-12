namespace TorznabClient.Models;

public record TorznabCategory
{
    private TorznabCategory()
    {
        Subcats = [];
    }

    [XmlAttribute("id")]
    public int Id { get; init; }

    [XmlAttribute("name")]
    public string? Name { get; init; }

    [XmlElement("subcat")]
    public List<TorznabSubcat> Subcats { get; init; }
}