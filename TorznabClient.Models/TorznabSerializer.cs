namespace TorznabClient.Models;

public class TorznabSerializer<TSerializer> : ITorznabSerializer<TSerializer>
{
    private readonly XmlSerializer _serializer = new(typeof(TSerializer));

    public TSerializer? Deserialize(string xml)
    {
        using var reader = new StringReader(xml);
        return (TSerializer?)_serializer.Deserialize(reader);
    }
}