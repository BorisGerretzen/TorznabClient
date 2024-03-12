namespace TorznabClient.Models;

[XmlRoot("error")]
public record TorznabError
{
    private TorznabError()
    {
    }

    [XmlAttribute("code")]
    public int Code { get; init; }

    [XmlAttribute("description")]
    public string? Description { get; init; }
}