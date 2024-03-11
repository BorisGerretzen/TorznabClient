using Microsoft.Extensions.Options;
using TorznabClient.Jackett;

namespace TorznabClient.Test;

public class JackettClientTest
{
    private const string BaseAddress = "http://localhost:9117/";
    private const string ApiKey = "test_key";
    private IOptions<JackettClientOptions> _options = null!;

    [SetUp]
    public void Setup()
    {
        var options = new JackettClientOptions
        {
            Url = BaseAddress,
            ApiKey = ApiKey
        };
        _options = Options.Create(options);
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
        var httpClient = TestHelpers.GetMockedClient(xml, BaseAddress, expectedUrl: BaseAddress + $"api/v2.0/indexers/all/results/torznab?t=indexers&apikey={ApiKey}&configured=True");
        var torznabClient = new TorznabClient(_options, httpClient);
        var jackettClient = new JackettClient(torznabClient, httpClient, _options);

        var response = await jackettClient.GetIndexersAsync(configured: true);

        Assert.That(response, Is.Not.Null);
        Assert.That(response.Count, Is.EqualTo(2));
        Assert.That(response[0].Id, Is.EqualTo("acgrip"));
        Assert.That(response[1].Id, Is.EqualTo("bitsearch"));
    }
}