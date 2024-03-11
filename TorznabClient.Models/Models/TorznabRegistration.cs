namespace TorznabClient.Models.Models;

public record TorznabRegistration
{
    private TorznabRegistration()
    {
    }

    public bool Available { get; init; }

    [XmlAttribute("available")]
    public string AvailableString
    {
        get => Available ? "yes" : "no";
        init => Available = value == "yes";
    }

    public bool Open { get; init; }

    [XmlAttribute("open")]
    public string OpenString
    {
        get => Open ? "yes" : "no";
        init => Open = value == "yes";
    }
}