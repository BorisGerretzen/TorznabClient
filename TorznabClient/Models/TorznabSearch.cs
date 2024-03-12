namespace TorznabClient.Models;

public record TorznabSearch
{
    private TorznabSearch()
    {
    }

    [XmlIgnore]
    public bool Available { get; init; }

    [XmlAttribute("available")]
    public string AvailableString
    {
        get => Available ? "yes" : "no";
        init => Available = value == "yes";
    }

    [XmlAttribute("supportedParams")]
    public string? SupportedParams { get; init; }
}