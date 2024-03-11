namespace TorznabClient.Models.Models;

public record TorznabChannel(TorznabAtomLink AtomLink, string Title, string Description, string Link, string Language, string Category, IEnumerable<TorznabRelease> Item);