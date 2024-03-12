namespace TorznabClient.Models;

public record TorznabSearching
{
    private TorznabSearching()
    {
    }

    [XmlElement("search")]
    public TorznabSearch? Search { get; init; }

    [XmlElement("tv-search")]
    public TorznabSearch? TvSearch { get; init; }

    [XmlElement("movie-search")]
    public TorznabSearch? MovieSearch { get; init; }

    [XmlElement("music-search")]
    public TorznabSearch? MusicSearch { get; init; }

    [XmlElement("audio-search")]
    public TorznabSearch? AudioSearch { get; init; }

    [XmlElement("book-search")]
    public TorznabSearch? BookSearch { get; init; }
}