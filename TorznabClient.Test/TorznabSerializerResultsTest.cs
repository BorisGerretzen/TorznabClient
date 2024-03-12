using TorznabClient.Models;
using TorznabClient.Serializer;

namespace TorznabClient.Test;

public class TorznabSerializerResultsTest
{
    private TorznabSerializer<TorznabRss> _torznabSerializer = null!;

    [SetUp]
    public void Setup()
    {
        _torznabSerializer = new TorznabSerializer<TorznabRss>();
    }

    [Test]
    public void TestSingle()
    {
        const string xml = """
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
        var rss = _torznabSerializer.Deserialize(xml);

        Assert.That(rss, Is.Not.Null);
        Assert.That(rss!.Channel, Is.Not.Null);

        var channel = rss.Channel!;
        Assert.That(channel.AtomLink, Is.Not.Null);
        Assert.That(channel.AtomLink!.Href, Is.EqualTo("http://127.0.0.1:9117/"));
        Assert.That(channel.AtomLink!.Rel, Is.EqualTo("self"));
        Assert.That(channel.AtomLink!.Type, Is.EqualTo("application/rss+xml"));

        Assert.That(channel.Title, Is.EqualTo("AggregateSearch"));
        Assert.That(channel.Description, Is.EqualTo("This feed includes all configured trackers"));
        Assert.That(channel.Link, Is.EqualTo("http://127.0.0.1/"));
        Assert.That(channel.Language, Is.EqualTo("en-US"));
        Assert.That(channel.Category, Is.EqualTo("search"));

        Assert.That(channel.Releases, Is.Not.Null);
        Assert.That(channel.Releases, Has.Count.EqualTo(1));

        var item = channel.Releases[0];
        Assert.That(item.Title, Is.EqualTo("vid1"));
        Assert.That(item.Guid, Is.EqualTo("vid1.mkv"));
        Assert.That(item.Type, Is.EqualTo("public"));
        Assert.That(item.Comments, Is.EqualTo("comments"));
        Assert.That(item.PubDateString, Is.EqualTo("Sat, 09 Dec 2023 00:00:00 +0100"));
        Assert.That(item.Size, Is.EqualTo(1610612736));
        Assert.That(item.Grabs, Is.EqualTo(1200));
        Assert.That(item.Description, Is.EqualTo(string.Empty));
        Assert.That(item.Link, Is.EqualTo("..."));
        Assert.That(item.Categories, Is.EquivalentTo(new[] { 5000 }));
        Assert.That(item.PubDateString, Is.EqualTo("Sat, 09 Dec 2023 00:00:00 +0100"));
        Assert.That(item.PubDate, Is.EqualTo(new DateTimeOffset(2023, 12, 9, 0, 0, 0, TimeSpan.FromHours(1))));
        Assert.That(item.Enclosure, Is.Not.Null);
        Assert.That(item.Enclosure!.Url, Is.EqualTo("..."));
        Assert.That(item.Enclosure!.Length, Is.EqualTo(1610612736));
        Assert.That(item.Enclosure!.Type, Is.EqualTo("application/x-bittorrent"));
        Assert.That(item.Attributes, Has.Count.EqualTo(8));
        Assert.That(item.Attributes[0].Name, Is.EqualTo("category"));
        Assert.That(item.Attributes[0].Value, Is.EqualTo("5000"));
        Assert.That(item.Attributes[1].Name, Is.EqualTo("genre"));
        Assert.That(item.Attributes[1].Value, Is.EqualTo(""));
        Assert.That(item.Attributes[2].Name, Is.EqualTo("seeders"));
        Assert.That(item.Attributes[2].Value, Is.EqualTo("84"));
        Assert.That(item.Attributes[3].Name, Is.EqualTo("peers"));
        Assert.That(item.Attributes[3].Value, Is.EqualTo("165"));
        Assert.That(item.Attributes[4].Name, Is.EqualTo("infohash"));
        Assert.That(item.Attributes[4].Value, Is.EqualTo("38C747C92231B893C6C00CDD8C1E32F6547E21B0"));
        Assert.That(item.Attributes[5].Name, Is.EqualTo("magneturl"));
        Assert.That(item.Attributes[5].Value, Is.EqualTo("..."));
        Assert.That(item.Attributes[6].Name, Is.EqualTo("downloadvolumefactor"));
        Assert.That(item.Attributes[6].Value, Is.EqualTo("0"));
        Assert.That(item.Attributes[7].Name, Is.EqualTo("uploadvolumefactor"));
        Assert.That(item.Attributes[7].Value, Is.EqualTo("1"));
    }
}