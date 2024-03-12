using System.Globalization;
using TorznabClient.Serializer;

namespace TorznabClient.Models;

public record TorznabGroup
{
    [XmlAttribute("id")]
    public int Id { get; init; }

    [XmlAttribute("name")]
    public string? Name { get; init; }

    [XmlAttribute("description")]
    public string? Description { get; init; }

    [XmlAttribute("lastupdate")]
    public string? LastUpdateString { get; init; }

    [XmlIgnore]
    public DateTimeOffset? LastUpdate
    {
        get => LastUpdateString == null
            ? null
            : DateTimeOffset.ParseExact(LastUpdateString, SerializationConstants.DateFormat, CultureInfo.InvariantCulture);
        init => LastUpdateString = value?.ToString(SerializationConstants.DateFormat);
    }
}