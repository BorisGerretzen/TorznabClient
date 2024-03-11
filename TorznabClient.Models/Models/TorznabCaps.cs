namespace TorznabClient.Models.Models;

public record TorznabCaps(TorznabServer Server, TorznabSearching Searching, IEnumerable<TorznabCategory> Categories);