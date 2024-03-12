using TorznabClient.Models;
using TorznabClient.Serializer;

namespace TorznabClient.Test;

public class TorznabSerializerIndexersTest
{
    private TorznabSerializer<TorznabIndexers> _torznabSerializer = null!;

    [SetUp]
    public void Setup()
    {
        _torznabSerializer = new TorznabSerializer<TorznabIndexers>();
    }

    [Test]
    public void TestIndexers()
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
        var indexers = _torznabSerializer.Deserialize(xml)?.Indexers;

        Assert.That(indexers, Is.Not.Null);
        Assert.That(indexers, Has.Count.EqualTo(2));

        var acgrip = indexers![0];
        Assert.That(acgrip, Is.Not.Null);
        Assert.That(acgrip.Id, Is.EqualTo("acgrip"));
        Assert.That(acgrip.Configured, Is.True);
        Assert.That(acgrip.Title, Is.EqualTo("ACG.RIP"));
        Assert.That(acgrip.Description, Is.EqualTo("ACG.RIP is a CHINESE Public torrent tracker for the latest anime and Japanese related torrents"));
        Assert.That(acgrip.Link, Is.EqualTo("https://acg.rip/"));
        Assert.That(acgrip.Language, Is.EqualTo("zh-CN"));
        Assert.That(acgrip.Type, Is.EqualTo("public"));
        Assert.That(acgrip.Caps, Is.Not.Null);
        Assert.That(acgrip.Caps!.Server, Is.Not.Null);
        Assert.That(acgrip.Caps!.Server!.Title, Is.EqualTo("Jackett"));

        var bitsearch = indexers[1];
        Assert.That(bitsearch, Is.Not.Null);
        Assert.That(bitsearch.Id, Is.EqualTo("bitsearch"));
        Assert.That(bitsearch.Configured, Is.True);
        Assert.That(bitsearch.Title, Is.EqualTo("BitSearch"));
        Assert.That(bitsearch.Description, Is.EqualTo("BitSearch is a Public torrent meta-search engine"));
        Assert.That(bitsearch.Link, Is.EqualTo("https://bitsearch.to/"));
        Assert.That(bitsearch.Language, Is.EqualTo("en-US"));
        Assert.That(bitsearch.Type, Is.EqualTo("public"));
        Assert.That(bitsearch.Caps, Is.Not.Null);
        Assert.That(bitsearch.Caps!.Server, Is.Not.Null);
        Assert.That(bitsearch.Caps!.Server!.Title, Is.EqualTo("Jackett"));
    }
}