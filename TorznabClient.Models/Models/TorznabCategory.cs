namespace TorznabClient.Models.Models;

public record TorznabCategory(int Id, string Name, IEnumerable<TorznabSubcat> Subcats);