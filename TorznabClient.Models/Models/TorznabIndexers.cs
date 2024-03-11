namespace TorznabClient.Models.Models;

[XmlRoot("indexers")]
public record TorznabIndexers
{
    private TorznabIndexers()
    {
        Indexers = [];
    }

    [XmlElement("indexer")] public List<TorznabIndexer> Indexers { get; init; }
}