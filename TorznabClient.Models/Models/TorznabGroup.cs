using System.Globalization;

namespace TorznabClient.Models.Models;

public record TorznabGroup
{
    [XmlAttribute("id")] public int Id { get; init; }

    [XmlAttribute("name")] public string? Name { get; init; }

    [XmlAttribute("description")] public string? Description { get; init; }

    [XmlAttribute("lastupdate")] public string? LastUpdateString { get; init; }

    [XmlIgnore]
    public DateTime? LastUpdate
    {
        get => LastUpdateString == null ? null : DateTime.ParseExact(LastUpdateString, Constants.DateFormat, CultureInfo.InvariantCulture);
        init => LastUpdateString = value?.ToString(Constants.DateFormat);
    }
}