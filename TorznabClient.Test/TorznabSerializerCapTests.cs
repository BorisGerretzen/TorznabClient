using TorznabClient.Exceptions;
using TorznabClient.Models;
using TorznabClient.Serializer;

namespace TorznabClient.Test;

public class TorznabSerializerCapTests
{
    private TorznabSerializer<TorznabCaps> _torznabSerializer = null!;

    [SetUp]
    public void Setup()
    {
        _torznabSerializer = new TorznabSerializer<TorznabCaps>();
    }

    /// <summary>
    ///     https://torznab.github.io/spec-1.3-draft/torznab/Specification-v1.3.html#caps-format
    /// </summary>
    [Test]
    public void DeserializesCapsExample()
    {
        const string xml = """
                           <?xml version="1.0" encoding="UTF-8"?>
                           <caps>
                              <server version="1.1" title="..." strapline="..."
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
        var deserialized = _torznabSerializer.Deserialize(xml);

        Assert.That(deserialized, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(deserialized!.Server, Is.Not.Null);
            Assert.That(deserialized.Server?.Title, Is.EqualTo("..."));
            Assert.That(deserialized.Server?.Strapline, Is.EqualTo("..."));
            Assert.That(deserialized.Server?.Email, Is.EqualTo("..."));
            Assert.That(deserialized.Server?.Url, Is.EqualTo("http://indexer.local/"));
            Assert.That(deserialized.Server?.Image, Is.EqualTo("http://indexer.local/content/banner.jpg"));
        });

        Assert.That(deserialized!.Limits, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.Limits?.Default, Is.EqualTo(50));
            Assert.That(deserialized.Limits?.Max, Is.EqualTo(100));
        });

        Assert.That(deserialized.Retention, Is.Not.Null);
        Assert.That(deserialized.Retention?.Days, Is.EqualTo(400));

        Assert.That(deserialized.Registration, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.Registration?.Available, Is.True);
            Assert.That(deserialized.Registration?.Open, Is.True);
        });

        Assert.That(deserialized.Searching, Is.Not.Null);

        // ReSharper disable once CyclomaticComplexity
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.Searching?.Search?.Available, Is.True);
            Assert.That(deserialized.Searching?.Search?.SupportedParams, Is.EqualTo("q"));
            Assert.That(deserialized.Searching?.TvSearch?.Available, Is.True);
            Assert.That(deserialized.Searching?.TvSearch?.SupportedParams, Is.EqualTo("q,rid,tvdbid,season,ep"));
            Assert.That(deserialized.Searching?.MovieSearch?.Available, Is.False);
            Assert.That(deserialized.Searching?.MovieSearch?.SupportedParams, Is.EqualTo("q,imdbid,genre"));
            Assert.That(deserialized.Searching?.AudioSearch?.Available, Is.False);
            Assert.That(deserialized.Searching?.AudioSearch?.SupportedParams, Is.EqualTo("q"));
            Assert.That(deserialized.Searching?.BookSearch?.Available, Is.False);
            Assert.That(deserialized.Searching?.BookSearch?.SupportedParams, Is.EqualTo("q"));
            Assert.That(deserialized.Searching?.MusicSearch?.Available, Is.False);
            Assert.That(deserialized.Searching?.MusicSearch?.SupportedParams, Is.EqualTo("q"));
        });

        Assert.That(deserialized.Categories, Is.Not.Null);
        var categories = deserialized.Categories.ToList();
        Assert.That(categories, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(categories[0].Id, Is.EqualTo(2000));
            Assert.That(categories[0].Name, Is.EqualTo("Movies"));
            Assert.That(categories[0].Subcats, Has.Count.EqualTo(1));
            Assert.That(categories[0].Subcats[0].Id, Is.EqualTo(2010));
            Assert.That(categories[0].Subcats[0].Name, Is.EqualTo("Foreign"));

            Assert.That(categories[1].Id, Is.EqualTo(5000));
            Assert.That(categories[1].Name, Is.EqualTo("TV"));
            Assert.That(categories[1].Subcats, Has.Count.EqualTo(2));
            Assert.That(categories[1].Subcats[0].Id, Is.EqualTo(5040));
            Assert.That(categories[1].Subcats[0].Name, Is.EqualTo("HD"));
            Assert.That(categories[1].Subcats[1].Id, Is.EqualTo(5070));
            Assert.That(categories[1].Subcats[1].Name, Is.EqualTo("Anime"));
        });

        Assert.That(deserialized.Groups, Is.Not.Null);
        var groups = deserialized.Groups.ToList();
        Assert.That(groups, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(groups[0].Id, Is.EqualTo(1));
            Assert.That(groups[0].Name, Is.EqualTo("alt.binaries...."));
            Assert.That(groups[0].Description, Is.EqualTo("..."));
            Assert.That(groups[0].LastUpdateString, Is.EqualTo("Sat, 09 Dec 2023 00:00:00 +0100"));
            Assert.That(groups[0].LastUpdate, Is.EqualTo(new DateTimeOffset(2023, 12, 9, 0, 0, 0, TimeSpan.FromHours(1))));
        });

        Assert.That(deserialized.Genres, Is.Not.Null);
        var genres = deserialized.Genres.ToList();
        Assert.That(genres, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(genres[0].Id, Is.EqualTo(1));
            Assert.That(genres[0].CategoryId, Is.EqualTo(5000));
            Assert.That(genres[0].Name, Is.EqualTo("Kids"));
        });

        Assert.That(deserialized.Tags, Is.Not.Null);
        var tags = deserialized.Tags.ToList();
        Assert.That(tags, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(tags[0].Name, Is.EqualTo("anonymous"));
            Assert.That(tags[0].Description, Is.EqualTo("Uploader is anonymous"));
            Assert.That(tags[1].Name, Is.EqualTo("trusted"));
            Assert.That(tags[1].Description, Is.EqualTo("Uploader has high reputation"));
            Assert.That(tags[2].Name, Is.EqualTo("internal"));
            Assert.That(tags[2].Description, Is.EqualTo("Uploader is an internal release group"));
        });
    }

    [Test]
    public void ThrowsException()
    {
        var xml = """
                  <?xml version="1.0" encoding="UTF-8"?>
                  <error code="999" description="This is a description"/>
                  """;
        var exception = Assert.Throws<TorznabException>(() => _torznabSerializer.Deserialize(xml));
        Assert.That(exception?.Code, Is.EqualTo(999));
        Assert.That(exception?.Message, Is.EqualTo("This is a description"));
    }
}