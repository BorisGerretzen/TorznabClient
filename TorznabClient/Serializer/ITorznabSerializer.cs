namespace TorznabClient.Serializer;

public interface ITorznabSerializer<out TSerializer>
{
    TSerializer? Deserialize(string xml);
    TSerializer? Deserialize(Stream stream);
}