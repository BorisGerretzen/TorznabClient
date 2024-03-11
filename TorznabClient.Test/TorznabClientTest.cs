using Microsoft.Extensions.Options;
using TorznabClient.Models.Exceptions;

namespace TorznabClient.Test;

public class TorznabClientTest
{
    private const string BaseAddress = "http://localhost:9117/api/v2.0/indexers/torznab";
    private const string ApiKey = "test_key";

    private const string DefaultResponse = """
                                           <?xml version="1.0" encoding="UTF-8"?>
                                           <rss version="2.0" xmlns:atom="http://www.w3.org/2005/Atom" xmlns:torznab="http://torznab.com/schemas/2015/feed">
                                               <channel>
                                                   <atom:link href="http://127.0.0.1:9117/" rel="self" type="application/rss+xml"/>
                                                   <title>AggregateSearch</title>
                                                   <description>This feed includes all configured trackers</description>
                                                   <link>http://127.0.0.1/</link>
                                                   <language>en-US</language>
                                                   <category>search</category>
                                                   <item>
                                                       <title>vid1</title>
                                                       <guid>vid1.mkv</guid>
                                                       <jackettindexer id="bitsearch">BitSearch</jackettindexer>
                                                       <type>public</type>
                                                       <comments>comments</comments>
                                                       <pubDate>Sat, 09 Dec 2023 00:00:00 +0100</pubDate>
                                                       <size>1610612736</size>
                                                       <grabs>1200</grabs>
                                                       <description/>
                                                       <link>...</link>
                                                       <category>5000</category>
                                                       <pubDate>Sat, 09 Dec 2023 00:00:00 +0100</pubDate>
                                                       <enclosure url="..." length="1610612736" type="application/x-bittorrent"/>
                                                       <torznab:attr name="category" value="5000"/>
                                                       <torznab:attr name="genre" value=""/>
                                                       <torznab:attr name="seeders" value="84"/>
                                                       <torznab:attr name="peers" value="165"/>
                                                       <torznab:attr name="infohash" value="38C747C92231B893C6C00CDD8C1E32F6547E21B0"/>
                                                       <torznab:attr name="magneturl" value="..."/>
                                                       <torznab:attr name="downloadvolumefactor" value="0"/>
                                                       <torznab:attr name="uploadvolumefactor" value="1"/>
                                                   </item>
                                               </channel>
                                           </rss>
                                           """;

    private const string ErrorResponse = """
                                         <?xml version="1.0" encoding="UTF-8"?>
                                         <error code="999" description="This is a description"/>
                                         """;

    private IOptions<TorznabClientOptions> _options = null!;

    [SetUp]
    public void Setup()
    {
        var options = new TorznabClientOptions
        {
            Url = BaseAddress,
            ApiKey = ApiKey
        };
        _options = Options.Create(options);
    }


    [Test]
    public async Task TestCapsValid()
    {
        const string xml = """
                           <?xml version="1.0" encoding="UTF-8"?>
                           <caps>
                              <server version="1.1" title="abc" strapline="..."
                                    email="..." url="http://indexer.local/"
                                    image="http://indexer.local/content/banner.jpg" />
                              <limits max="100" default="50" />
                              <retention days="400" />
                              <registration available="yes" open="yes" />
                           
                              <searching>
                                 <search available="yes" supportedParams="q" />
                                 <tv-search available="yes" supportedParams="q,rid,tvdbid,season,ep" />
                                 <movie-search available="no" supportedParams="q,imdbid,genre" />
                                 <audio-search available="no" supportedParams="q" />
                                 <book-search available="no" supportedParams="q" />
                                 <music-search available="no" supportedParams="q" />
                              </searching>
                           
                              <categories>
                                 <category id="2000" name="Movies">
                                    <subcat id="2010" name="Foreign" />
                                 </category>
                                 <category id="5000" name="TV">
                                    <subcat id="5040" name="HD" />
                                    <subcat id="5070" name="Anime" />
                                 </category>
                              </categories>
                           
                              <groups>
                                 <group id="1" name="alt.binaries...." description="..." lastupdate="Sat, 09 Dec 2023 00:00:00 +0100" />
                              </groups>
                           
                              <genres>
                                 <genre id="1" categoryid="5000" name="Kids" />
                              </genres>
                           
                              <tags>
                                 <tag name="anonymous" description="Uploader is anonymous" />
                                 <tag name="trusted" description="Uploader has high reputation" />
                                 <tag name="internal" description="Uploader is an internal release group" />
                              </tags>
                           </caps>
                           """;
        var httpClient = TestHelpers.GetMockedClient(xml, BaseAddress, expectedUrl: BaseAddress + $"?t=caps&apikey={ApiKey}");
        var torznabClient = new TorznabClient(_options, httpClient);

        var response = await torznabClient.GetCapsAsync();

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Server, Is.Not.Null);
        Assert.That(response.Server?.Title, Is.EqualTo("abc"));
    }

    [Test]
    public async Task TestSearchValid()
    {
        var httpClient = TestHelpers.GetMockedClient(DefaultResponse, BaseAddress, expectedUrl: BaseAddress + $"?t=search&apikey={ApiKey}&q=test&group=a%2cb&cat=10%2c20&maxsize=1000");
        var torznabClient = new TorznabClient(_options, httpClient);
        var response = await torznabClient.SearchAsync(query: "test", groups: ["a", "b"], maxSize: 1000, categories: [10, 20]);

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Channel?.Title, Is.EqualTo("AggregateSearch"));
    }

    [Test]
    public async Task TestTvSearch()
    {
        var httpClient = TestHelpers.GetMockedClient(DefaultResponse, BaseAddress, expectedUrl: BaseAddress + $"?t=tvsearch&apikey={ApiKey}&q=test&season=S01&ep=E02&rid=123&tvdbid=456");
        var torznabClient = new TorznabClient(_options, httpClient);
        var response = await torznabClient.TvSearchAsync(query: "test", tvRageId: "123", tvDbId: 456, season: "S01", episode: "E02");

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Channel?.Title, Is.EqualTo("AggregateSearch"));
    }

    [Test]
    public async Task TestMovieSearch()
    {
        var httpClient = TestHelpers.GetMockedClient(DefaultResponse, BaseAddress, expectedUrl: BaseAddress + $"?t=movie&apikey={ApiKey}&q=test&imdbid=tt123&cat=10%2c20&genre=action");
        var torznabClient = new TorznabClient(_options, httpClient);
        var response = await torznabClient.MovieSearchAsync(query: "test", imdbId: "tt123", genre: "action", categories: new[] { 10, 20 });

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Channel?.Title, Is.EqualTo("AggregateSearch"));
    }

    [Test]
    public void TestError()
    {
        var httpClient = TestHelpers.GetMockedClient(ErrorResponse, BaseAddress, expectedUrl: BaseAddress + $"?t=caps&apikey={ApiKey}");
        var torznabClient = new TorznabClient(_options, httpClient);

        var exception = Assert.ThrowsAsync<TorznabException>(async () => await torznabClient.GetCapsAsync());
        Assert.That(exception?.Code, Is.EqualTo(999));
        Assert.That(exception?.Message, Is.EqualTo("This is a description"));
    }

    [Test]
    public async Task TestGetIndexers()
    {
        const string xml = """
                           <?xml version="1.0" encoding="UTF-8"?>
                           <indexers>
                               <indexer id="acgrip" configured="true">
                                   <title>ACG.RIP</title>
                                   <description>ACG.RIP is a CHINESE Public torrent tracker for the latest anime and Japanese related torrents</description>
                                   <link>https://acg.rip/</link>
                                   <language>zh-CN</language>
                                   <type>public</type>
                                   <caps>
                                       <server title="Jackett"/>
                                       <limits default="100" max="100"/>
                                       <searching>
                                           <search available="yes" supportedParams="q"/>
                                           <tv-search available="yes" supportedParams="q,season,ep"/>
                                           <movie-search available="no" supportedParams="q"/>
                                           <music-search available="no" supportedParams="q"/>
                                           <audio-search available="no" supportedParams="q"/>
                                           <book-search available="no" supportedParams="q"/>
                                       </searching>
                                       <categories>
                                           <category id="5000" name="TV"/>
                                       </categories>
                                   </caps>
                               </indexer>
                               <indexer id="bitsearch" configured="true">
                                   <title>BitSearch</title>
                                   <description>BitSearch is a Public torrent meta-search engine</description>
                                   <link>https://bitsearch.to/</link>
                                   <language>en-US</language>
                                   <type>public</type>
                                   <caps>
                                       <server title="Jackett"/>
                                       <limits default="100" max="100"/>
                                       <searching>
                                           <search available="yes" supportedParams="q"/>
                                           <tv-search available="yes" supportedParams="q,season,ep"/>
                                           <movie-search available="yes" supportedParams="q"/>
                                           <music-search available="yes" supportedParams="q"/>
                                           <audio-search available="yes" supportedParams="q"/>
                                           <book-search available="yes" supportedParams="q"/>
                                       </searching>
                                       <categories>
                                           <category id="2000" name="Movies"/>
                                           <category id="3000" name="Audio">
                                               <subcat id="3010" name="Audio/MP3"/>
                                               <subcat id="3020" name="Audio/Video"/>
                                               <subcat id="3030" name="Audio/Audiobook"/>
                                               <subcat id="3040" name="Audio/Lossless"/>
                                           </category>
                                           <category id="4000" name="PC">
                                               <subcat id="4010" name="PC/0day"/>
                                               <subcat id="4020" name="PC/ISO"/>
                                               <subcat id="4050" name="PC/Games"/>
                                               <subcat id="4070" name="PC/Mobile-Android"/>
                                           </category>
                                           <category id="5000" name="TV"/>
                                           <category id="6000" name="XXX"/>
                                           <category id="7000" name="Books">
                                               <subcat id="7020" name="Books/EBook"/>
                                               <subcat id="7030" name="Books/Comics"/>
                                           </category>
                                           <category id="8000" name="Other">
                                               <subcat id="8010" name="Other/Misc"/>
                                           </category>
                                       </categories>
                                   </caps>
                               </indexer>
                           </indexers>
                           """;
        var httpClient = TestHelpers.GetMockedClient(xml, BaseAddress, expectedUrl: BaseAddress + $"?t=indexers&apikey={ApiKey}&configured=True");
        var torznabClient = new TorznabClient(_options, httpClient);

        var response = await torznabClient.GetIndexersAsync(configured: true);

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Count, Is.EqualTo(2));
        Assert.That(response[0].Id, Is.EqualTo("acgrip"));
        Assert.That(response[1].Id, Is.EqualTo("bitsearch"));
    }
}