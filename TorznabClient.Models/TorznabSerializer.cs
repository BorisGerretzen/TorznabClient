using System.Text;
using System.Xml.Serialization;

namespace TorznabClient.Models;

public class TorznabSerializer : ITorznabSerializer
{
    private readonly XmlSerializer _serializer;

    public TorznabSerializer(XmlSerializer serializer)
    {
        _serializer = serializer;
    }

    public string Serialize<T>(T obj)
    {
        var sb = new StringBuilder();
        using (var writer = new StringWriter(sb))
        {
            _serializer.Serialize(writer, obj);
        }

        return sb.ToString();
    }

    public T? Deserialize<T>(string xml)
    {
        using var reader = new StringReader(xml);
        return (T?)_serializer.Deserialize(reader);
    }
}