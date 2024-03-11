namespace TorznabClient.Models;

public interface ITorznabSerializer
{
    string Serialize<T>(T obj);
    T? Deserialize<T>(string xml);
}