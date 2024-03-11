namespace TorznabClient.Models.Models;

public record TorznabRetention
{
    private TorznabRetention()
    {
    }

    [XmlAttribute("days")] public int Days { get; init; }
}